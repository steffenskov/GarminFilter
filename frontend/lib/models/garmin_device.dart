class GarminDevice {
  final String name;
  final int id;

  GarminDevice({required this.name, required this.id});

  factory GarminDevice.fromJson(Map<String, dynamic> json) {
    return GarminDevice(name: json['name'] as String, id: json['id'] is int ? json['id'] as int : int.parse(json['id'].toString()));
  }
}
