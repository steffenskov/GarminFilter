import 'package:flutter/material.dart';

class Label extends StatelessWidget {
  const Label(this.text, {super.key});

  final String text;

  @override
  Widget build(BuildContext context) {
    return SizedBox(width: 150, child: Text(text));
  }
}
