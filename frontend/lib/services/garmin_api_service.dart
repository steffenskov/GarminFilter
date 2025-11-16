import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/garmin_device.dart';
import '../models/app_query_model.dart';
import '../models/app_view_model.dart';
import '../models/sync_status_view_model.dart';
import '../models/permission.dart';

class GarminApiService {
  static const String baseUrl = String.fromEnvironment('BACKEND_URL', defaultValue: 'http://localhost:8080');

  // Helper method to convert relative URLs to absolute URLs
  static String getAbsoluteImageUrl(String relativeUrl) {
    if (relativeUrl.startsWith('/')) {
      return '$baseUrl$relativeUrl';
    }
    return relativeUrl; // Already absolute
  }

  static Future<List<String>> getOrderByOptions() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/ordering'), headers: {'Content-Type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        print('Raw JSON response: $jsonList'); // Debug print
        return jsonList.map((json) {
          print('Processing orderBy JSON: $json'); // Debug print
          return json['name'] as String;
        }).toList();
      } else {
        throw Exception('Failed to load orderBy: ${response.statusCode}');
      }
    } catch (e) {
      print('Error details: $e'); // Debug print
      throw Exception('Error fetching orderBy: $e');
    }
  }

  // Get list of available devices
  static Future<List<GarminDevice>> getDevices() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/device'), headers: {'Content-Type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        print('Raw JSON response: $jsonList'); // Debug print
        return jsonList.map((json) {
          print('Processing device JSON: $json'); // Debug print
          return GarminDevice.fromJson(json);
        }).toList();
      } else {
        throw Exception('Failed to load devices: ${response.statusCode}');
      }
    } catch (e) {
      print('Error details: $e'); // Debug print
      throw Exception('Error fetching devices: $e');
    }
  }

  // Get watchfaces for a specific device
  static Future<List<AppViewModel>> getWatchfaces(int deviceId, AppQueryModel query) async {
    try {
      final response = await http.post(Uri.parse('$baseUrl/device/$deviceId/watchface'), headers: {'Content-Type': 'application/json'}, body: json.encode(query.toJson()));

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        return jsonList.map((json) => AppViewModel.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load watchfaces: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error fetching watchfaces: $e');
    }
  }

  // Get available permissions
  static Future<List<Permission>> getPermissions() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/permission'), headers: {'Content-Type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        return jsonList.map((json) => Permission.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load permissions: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error fetching permissions: $e');
    }
  }

  // Get sync status
  static Future<List<SyncStatusViewModel>> getSyncStatus() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/status'), headers: {'Content-Type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        return jsonList.map((json) => SyncStatusViewModel.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load sync status: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error fetching sync status: $e');
    }
  }

  static Future<List<String>> getDevelopers() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/developer'), headers: {'Content-Type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        print('Raw JSON response: $jsonList'); // Debug print
        return jsonList.map((json) {
          print('Processing developer JSON: $json'); // Debug print
          return json['name'] as String;
        }).toList();
      } else {
        throw Exception('Failed to load developers: ${response.statusCode}');
      }
    } catch (e) {
      print('Error details: $e'); // Debug print
      throw Exception('Error fetching developers: $e');
    }
  }
}
