import 'package:flutter/material.dart';

import '../models/garmin_device.dart';
import '../services/garmin_api_service.dart';
import '../services/preferences_service.dart';
import 'label.dart';

class DevicesWidget extends StatefulWidget {
  final ValueChanged<GarminDevice?> onSelected;

  const DevicesWidget({required this.onSelected, super.key});

  @override
  State<DevicesWidget> createState() => _DevicesWidgetState();
}

class _DevicesWidgetState extends State<DevicesWidget> {
  GarminDevice? _selectedValue;
  List<GarminDevice> _devices = [];
  bool _isLoading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadDevices();
  }

  Future<void> _loadDevices() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final devices = await GarminApiService.getDevices();
      setState(() {
        _devices = devices;
        _isLoading = false;
      });

      // Load selected device after devices are loaded
      final selectedDevice = await PreferencesService.device.loadAsync(devices);
      if (selectedDevice != null) {
        await _setSelectedValueAsync(selectedDevice);
      }
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _setSelectedValueAsync(GarminDevice? value) async {
    setState(() {
      _selectedValue = value;
    });
    await PreferencesService.device.saveAsync(value);
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
              ElevatedButton(onPressed: _loadDevices, child: const Text('Retry')),
            ],
          )
        else
          Row(
            children: [
              Label("Choose a device"),
              Expanded(
                child: Autocomplete(
                  optionsBuilder: (TextEditingValue textEditingValue) {
                    if (textEditingValue.text.isEmpty) {
                      return _devices;
                    }
                    return _devices.where((device) => device.name.toLowerCase().contains(textEditingValue.text.toLowerCase()));
                  },
                  displayStringForOption: (GarminDevice device) => device.name,
                  onSelected: (GarminDevice? value) async {
                    await _setSelectedValueAsync(value);
                  },
                  fieldViewBuilder: (context, textEditingController, focusNode, onFieldSubmitted) {
                    if (_selectedValue != null) {
                      textEditingController.text = _selectedValue!.name;
                    }
                    return TextFormField(
                      controller: textEditingController,
                      focusNode: focusNode,
                      decoration: InputDecoration(
                        labelText: 'Choose a device',
                        hintText: 'Type to search devices...',
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
