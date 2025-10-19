class Permission {
  final String permission;
  final String description;

  Permission({
    required this.permission,
    required this.description,
  });

  factory Permission.fromJson(Map<String, dynamic> json) {
    return Permission(
      permission: json['permission'] as String,
      description: json['description'] as String,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'permission': permission,
      'description': description,
    };
  }
}
