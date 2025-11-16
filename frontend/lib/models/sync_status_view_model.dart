class SyncStatusViewModel {
  final int count;
  final bool initialSyncCompleted;
  final String type;

  SyncStatusViewModel({required this.count, required this.initialSyncCompleted, required this.type});

  factory SyncStatusViewModel.fromJson(Map<String, dynamic> json) {
    return SyncStatusViewModel(count: json['count'] as int, initialSyncCompleted: json['initialSyncCompleted'] as bool, type: json['type'] as String);
  }
}
