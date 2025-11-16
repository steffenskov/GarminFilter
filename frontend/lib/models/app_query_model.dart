class AppQueryModel {
  final List<String> excludePermissions;
  final bool? paid;
  final String? developer;
  final int pageIndex;
  final int pageSize;
  final String orderBy;

  AppQueryModel({required this.excludePermissions, required this.paid, required this.developer, required this.pageIndex, required this.pageSize, required this.orderBy});

  Map<String, dynamic> toJson() {
    return {'excludePermissions': excludePermissions, 'paid': paid, 'developer': developer, 'pageIndex': pageIndex, 'pageSize': pageSize, 'orderBy': orderBy};
  }
}
