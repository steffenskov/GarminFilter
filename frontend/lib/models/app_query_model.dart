class AppQueryModel {
  final List<String> excludePermissions;
  final bool? paid;
  final int pageIndex;
  final int pageSize;
  final String orderBy;

  AppQueryModel({
    required this.excludePermissions,
    required this.paid,
    required this.pageIndex,
    required this.pageSize,
    required this.orderBy,
  });

  factory AppQueryModel.fromJson(Map<String, dynamic> json) {
    return AppQueryModel(
      excludePermissions: List<String>.from(json['excludePermissions']),
      paid: json['includePaid'] as bool?,
      pageIndex: json['pageIndex'] as int,
      pageSize: json['pageSize'] as int,
      orderBy: json['orderBy'] as String,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'excludePermissions': excludePermissions,
      'paid': paid,
      'pageIndex': pageIndex,
      'pageSize': pageSize,
      'orderBy': orderBy,
    };
  }
}
