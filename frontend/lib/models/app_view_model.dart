import 'permission.dart';

class AppViewModel {
  final List<Permission> permissions;
  final bool isPaid;
  final String url;
  final String imageUrl;
  final String type;
  final String id;
  final String name;
  final String developer;
  final double averageRating;
  final int reviewCount;
  final String releaseDate;

  AppViewModel({
    required this.permissions,
    required this.isPaid,
    required this.url,
    required this.imageUrl,
    required this.type,
    required this.id,
    required this.name,
    required this.developer,
    required this.averageRating,
    required this.reviewCount,
    required this.releaseDate
  });

  factory AppViewModel.fromJson(Map<String, dynamic> json) {
    return AppViewModel(
      permissions: json['permissions'] != null 
          ? (json['permissions'] as List).map((p) => Permission.fromJson(p)).toList()
          : [],
      isPaid: json['isPaid'] as bool,
      url: json['url'] as String,
      imageUrl: json['imageUrl'] as String,
      type: json['type'] as String,
      id: json['id'] as String,
      name: json['name'] as String,
      developer: json['developer'] as String,
      averageRating: (json['averageRating'] as num?)?.toDouble() ?? 0.0,
      reviewCount: json['reviewCount'] as int? ?? 0,
      releaseDate: json['releaseDate'] as String
    );
  }
}
