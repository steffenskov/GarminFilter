import 'package:flutter/material.dart';

import '../services/garmin_api_service.dart';
import '../services/preferences_service.dart';
import 'label.dart';

class DevelopersWidget extends StatefulWidget {
  final ValueChanged<String?> onSelected;

  const DevelopersWidget({required this.onSelected, super.key});

  @override
  State<DevelopersWidget> createState() => _DevelopersWidgetState();
}

class _DevelopersWidgetState extends State<DevelopersWidget> {
  String? _selectedValue;
  List<String> _developers = [];
  bool _isLoading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadDevelopers();
  }

  Future<void> _loadDevelopers() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final developers = await GarminApiService.getDevelopers();
      setState(() {
        _developers = developers;
        _isLoading = false;
      });

      // Load selected developer after developers are loaded
      final selectedDeveloper = await PreferencesService.developer.loadAsync(developers);
      if (selectedDeveloper != null) {
        await _setSelectedValueAsync(selectedDeveloper);
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
    await PreferencesService.developer.saveAsync(value);
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
              ElevatedButton(onPressed: _loadDevelopers, child: const Text('Retry')),
            ],
          )
        else
          Row(
            children: [
              Label("Choose a developer"),
              Expanded(
                child: Autocomplete(
                  optionsBuilder: (TextEditingValue textEditingValue) {
                    if (textEditingValue.text.isEmpty) {
                      return _developers;
                    }
                    return _developers.where((developer) => developer.toLowerCase().contains(textEditingValue.text.toLowerCase()));
                  },
                  onSelected: (String? value) async {
                    await this._setSelectedValueAsync(value);
                  },
                  fieldViewBuilder: (context, textEditingController, focusNode, onFieldSubmitted) {
                    if (_selectedValue != null) {
                      textEditingController.text = _selectedValue!;
                    }
                    return TextFormField(
                      controller: textEditingController,
                      focusNode: focusNode,
                      decoration: InputDecoration(
                        labelText: 'Choose a developer',
                        hintText: 'Type to search developers...',
                        border: const OutlineInputBorder(),
                        suffixIcon: _selectedValue != null
                            ? IconButton(
                                icon: const Icon(Icons.clear),
                                onPressed: () async {
                                  textEditingController.clear();
                                  await _setSelectedValueAsync(null);
                                },
                              )
                            : null,
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
      ],
    );
  }
}
