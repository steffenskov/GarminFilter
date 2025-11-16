import 'package:flutter/material.dart';

import '../services/preferences_service.dart';
import 'label.dart';

class PaidWidget extends StatefulWidget {
  final ValueChanged<bool?> onSelected;

  const PaidWidget({required this.onSelected, super.key});

  @override
  State<PaidWidget> createState() => _PaidWidgetState();
}

class _PaidWidgetState extends State<PaidWidget> {
  bool? _selectedValue;

  @override
  void initState() {
    super.initState();
    _loadState();
  }

  Future<void> _loadState() async {
    final state = await PreferencesService.paid.loadAsync();
    await _setSelectedValueAsync(state);
  }

  Future<void> _setSelectedValueAsync(bool? value) async {
    setState(() {
      _selectedValue = value;
    });
    await PreferencesService.paid.saveAsync(value);
    widget.onSelected.call(value);
  }

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Label("Pricing model"),
        DropdownMenu<bool?>(
          dropdownMenuEntries: const [
            DropdownMenuEntry(label: "Both free and paid", value: null),
            DropdownMenuEntry(label: "Only free", value: false),
            DropdownMenuEntry(label: "Only paid", value: true),
          ],
          initialSelection: _selectedValue,
          onSelected: (bool? value) async {
            await _setSelectedValueAsync(value);
          },
        ),
      ],
    );
  }
}
