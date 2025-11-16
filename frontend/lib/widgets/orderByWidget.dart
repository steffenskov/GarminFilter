import 'package:flutter/material.dart';

import '../services/garmin_api_service.dart';
import '../services/preferences_service.dart';
import 'label.dart';

class OrderByWidget extends StatefulWidget {
  final ValueChanged<String?> onSelected;

  const OrderByWidget({required this.onSelected, super.key});

  @override
  State<OrderByWidget> createState() => _OrderByWidgetState();
}

class _OrderByWidgetState extends State<OrderByWidget> {
  String? _selectedValue;
  List<String> _options = [];
  bool _isLoading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadOrderByOptions();
  }

  Future<void> _loadOrderByOptions() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final orderByOptions = await GarminApiService.getOrderByOptions();
      setState(() {
        _options = orderByOptions;
        _isLoading = false;
      });

      // Load selected orderBy after orderBy options are loaded
      final selectedOrderBy = await PreferencesService.orderBy.loadAsync(orderByOptions);
      if (selectedOrderBy != null) {
        await _setSelectedValueAsync(selectedOrderBy);
      } else {
        await _setSelectedValueAsync(orderByOptions.first);
      }
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _setSelectedValueAsync(String? value) async {
    setState(() {
      _selectedValue = value;
    });
    await PreferencesService.orderBy.saveAsync(value);
    widget.onSelected.call(value);
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (_isLoading)
          const Center(child: CircularProgressIndicator())
        else if (_error != null)
          Column(
            children: [
              Text('Error: $_error', style: TextStyle(color: Colors.red[700])),
              const SizedBox(height: 8),
              ElevatedButton(onPressed: _loadOrderByOptions, child: const Text('Retry')),
            ],
          )
        else
          Row(
            children: [
              Label("Order by"),
              DropdownMenu<String>(
                dropdownMenuEntries: _options.map((item) => DropdownMenuEntry(label: item, value: item)).toList(),
                initialSelection: _selectedValue,
                onSelected: (String? value) async {
                  await _setSelectedValueAsync(value);
                },
              ),
            ],
          ),
      ],
    );
  }
}
