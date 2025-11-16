import 'package:flutter/material.dart';

import '../models/permission.dart';
import '../services/garmin_api_service.dart';
import '../services/preferences_service.dart';

class PermissionsWidget extends StatefulWidget {
  final ValueChanged<List<String>> onSelected;

  const PermissionsWidget({required this.onSelected, super.key});

  @override
  State<PermissionsWidget> createState() => _PermissionsWidgetState();
}

class _PermissionsWidgetState extends State<PermissionsWidget> {
  List<Permission> _availablePermissions = [];
  List<String> _selectedValue = [];
  bool _isLoading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadPermissions();
  }

  Future<void> _loadPermissions() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final permissions = await GarminApiService.getPermissions();
      setState(() {
        _availablePermissions = permissions;
        _isLoading = false;
      });
      final selectedValue = await PreferencesService.permissions.loadAsync();
      setState(() {
        _selectedValue = selectedValue;
      });
      widget.onSelected.call(selectedValue);
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
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
              Text('Error loading permissions: $_error', style: TextStyle(color: Colors.red[700])),
              const SizedBox(height: 8),
              ElevatedButton(onPressed: _loadPermissions, child: const Text('Retry')),
            ],
          )
        else if (_availablePermissions.isEmpty)
          const Text('No permissions available')
        else
          ExpansionTile(
            title: Text("Excluded permissions: ${_selectedValue.length}"),
            tilePadding: EdgeInsets.zero,
            children: _availablePermissions
                .map(
                  (permission) => CheckboxListTile(
                    title: Text(permission.description),
                    value: _selectedValue.contains(permission.permission),
                    onChanged: (bool? value) async {
                      setState(() {
                        if (value == true) {
                          _selectedValue.add(permission.permission);
                        } else {
                          _selectedValue.remove(permission.permission);
                        }
                      });
                      await PreferencesService.permissions.saveAsync(_selectedValue);
                      widget.onSelected.call(_selectedValue);
                    },
                  ),
                )
                .toList(),
          ),
      ],
    );
  }
}
