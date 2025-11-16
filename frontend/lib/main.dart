import 'package:flutter/material.dart';
import 'package:garmin_filter/widgets/developersWidget.dart';
import 'package:garmin_filter/widgets/devicesWidget.dart';
import 'package:garmin_filter/widgets/orderByWidget.dart';
import 'package:garmin_filter/widgets/paidWidget.dart';
import 'package:garmin_filter/widgets/permissionsWidget.dart';
import 'package:url_launcher/url_launcher.dart';

import 'models/app_query_model.dart';
import 'models/app_view_model.dart';
import 'models/garmin_device.dart';
import 'services/garmin_api_service.dart';

void main() {
  runApp(const GarminFilterApp());
}

class GarminFilterApp extends StatelessWidget {
  const GarminFilterApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Garmin Filter',
      theme: ThemeData(colorScheme: ColorScheme.fromSeed(seedColor: Colors.blue), useMaterial3: true),
      home: const GarminFilterHomePage(),
    );
  }
}

class GarminFilterHomePage extends StatefulWidget {
  const GarminFilterHomePage({super.key});

  @override
  State<GarminFilterHomePage> createState() => _GarminFilterHomePageState();
}

class _GarminFilterHomePageState extends State<GarminFilterHomePage> {
  String? _selectedDeveloper;
  GarminDevice? _selectedDevice;
  String? _selectedOrderBy;
  bool? _paid;
  List<String> _selectedExcludePermissions = [];

  List<AppViewModel> _watchfaces = [];
  bool _isLoadingWatchfaces = false;
  String? _watchfaceError;
  int _currentPageIndex = 0;
  bool _hasMoreData = true;
  final ScrollController _scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    _scrollController.addListener(_onScroll);
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }

  void _onScroll() {
    if (_scrollController.position.pixels >= _scrollController.position.maxScrollExtent - 200) {
      _loadMoreWatchfaces();
    }
  }

  Future<void> _searchWatchfaces({bool resetPagination = true}) async {
    if (_selectedOrderBy == null) {
      return;
    }

    if (_selectedDevice == null) {
      return;
    }

    if (resetPagination) {
      setState(() {
        _currentPageIndex = 0;
        _hasMoreData = true;
        _watchfaces = [];
        _isLoadingWatchfaces = true;
        _watchfaceError = null;
      });
    } else {
      setState(() {
        _isLoadingWatchfaces = true;
      });
    }

    try {
      final query = AppQueryModel(excludePermissions: _selectedExcludePermissions, paid: _paid, developer: _selectedDeveloper, pageIndex: _currentPageIndex, pageSize: 30, orderBy: _selectedOrderBy!);

      final watchfaces = await GarminApiService.getWatchfaces(_selectedDevice!.id, query);

      setState(() {
        if (resetPagination) {
          _watchfaces = watchfaces;
        } else {
          _watchfaces.addAll(watchfaces);
        }

        // Check if we got less than pageSize items, meaning we've reached the end
        _hasMoreData = watchfaces.length >= 30;
        _currentPageIndex++;
        _isLoadingWatchfaces = false;
      });
    } catch (e) {
      setState(() {
        _watchfaceError = e.toString();
        _isLoadingWatchfaces = false;
      });
    }
  }

  Future<void> _loadMoreWatchfaces() async {
    if (_selectedDevice != null && _hasMoreData && !_isLoadingWatchfaces) {
      await _searchWatchfaces(resetPagination: false);
    }
  }

  Future<void> _openUrl(String url) async {
    final uri = Uri.parse(url);
    if (await canLaunchUrl(uri)) {
      await launchUrl(uri, mode: LaunchMode.externalApplication);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(backgroundColor: Theme.of(context).colorScheme.inversePrimary, title: const Text('Garmin Filter'), centerTitle: true),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // Device Selector
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text('Filter options', style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                    const SizedBox(height: 8),
                    DevicesWidget(
                      onSelected: (GarminDevice? device) {
                        setState(() {
                          _selectedDevice = device;
                        });

                        _searchWatchfaces(resetPagination: true);
                      },
                    ),
                    SizedBox(height: 8),
                    DevelopersWidget(
                      onSelected: (String? value) {
                        setState(() {
                          _selectedDeveloper = value;
                        });
                        _searchWatchfaces(resetPagination: true);
                      },
                    ),
                    SizedBox(height: 8),
                    PaidWidget(
                      onSelected: (bool? value) {
                        setState(() {
                          _paid = value;
                        });
                        _searchWatchfaces(resetPagination: true);
                      },
                    ),
                    SizedBox(height: 8),

                    PermissionsWidget(
                      onSelected: (value) {
                        setState(() {
                          _selectedExcludePermissions = value;
                        });
                        _searchWatchfaces(resetPagination: true);
                      },
                    ),
                    const SizedBox(height: 8),
                    OrderByWidget(
                      onSelected: (value) {
                        setState(() {
                          _selectedOrderBy = value;
                        });
                        _searchWatchfaces(resetPagination: true);
                      },
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Watchface Results
            Expanded(
              child: _selectedDevice == null
                  ? Center(
                      child: Text('Please select a device to search for watchfaces', style: Theme.of(context).textTheme.bodyLarge, textAlign: TextAlign.center),
                    )
                  : _isLoadingWatchfaces &&
                        _watchfaces
                            .isEmpty // Only show spinner if initial load
                  ? const Center(child: CircularProgressIndicator())
                  : _watchfaceError != null
                  ? Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Text(
                            'Error: $_watchfaceError',
                            style: TextStyle(color: Colors.red[700]),
                            textAlign: TextAlign.center,
                          ),
                          const SizedBox(height: 16),
                          ElevatedButton(onPressed: () => _searchWatchfaces(), child: const Text('Retry')),
                        ],
                      ),
                    )
                  : _watchfaces.isEmpty
                  ? Center(
                      child: Text('No watchfaces found', style: Theme.of(context).textTheme.bodyLarge, textAlign: TextAlign.center),
                    )
                  : ListView.builder(
                      controller: _scrollController,
                      itemCount: _watchfaces.length + (_hasMoreData ? 1 : 0),
                      itemBuilder: (context, index) {
                        // Show loading indicator at the bottom
                        if (index == _watchfaces.length) {
                          return const Padding(
                            padding: EdgeInsets.all(16.0),
                            child: Center(child: CircularProgressIndicator()),
                          );
                        }

                        final watchface = _watchfaces[index];
                        return Card(
                          margin: const EdgeInsets.symmetric(horizontal: 0, vertical: 4),
                          child: InkWell(
                            onTap: () => _openUrl(watchface.url),
                            child: Padding(
                              padding: const EdgeInsets.all(16.0),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  ClipRRect(
                                    borderRadius: BorderRadius.circular(8),
                                    child: Image.network(
                                      GarminApiService.getAbsoluteImageUrl(watchface.imageUrl),
                                      width: 160,
                                      height: 160,
                                      fit: BoxFit.cover,
                                      errorBuilder: (context, error, stackTrace) {
                                        return Container(width: 160, height: 160, color: Colors.grey[300], child: const Icon(Icons.image_not_supported));
                                      },
                                    ),
                                  ),
                                  const SizedBox(width: 16),
                                  Expanded(
                                    child: Column(
                                      crossAxisAlignment: CrossAxisAlignment.start,
                                      children: [
                                        Row(
                                          children: [
                                            Expanded(
                                              child: Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: [
                                                  Text(watchface.name, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                                                  const SizedBox(height: 2),
                                                  Text(watchface.developer, style: const TextStyle(fontSize: 12, color: Colors.grey)),
                                                  const SizedBox(height: 2),
                                                  Text(watchface.releaseDate, style: const TextStyle(fontSize: 12, color: Colors.grey)),
                                                  const SizedBox(height: 2),
                                                  Row(
                                                    children: [
                                                      Text('${watchface.averageRating.toStringAsFixed(1)} ', style: const TextStyle(fontSize: 12, color: Colors.grey)),
                                                      const Icon(Icons.star, size: 12, color: Colors.amber),
                                                      Text(' (${watchface.reviewCount})', style: const TextStyle(fontSize: 12, color: Colors.grey)),
                                                    ],
                                                  ),
                                                ],
                                              ),
                                            ),
                                            const Icon(Icons.open_in_new, size: 16),
                                          ],
                                        ),
                                        const SizedBox(height: 8),
                                        Row(
                                          children: [
                                            Icon(watchface.isPaid ? Icons.paid : Icons.check_circle, size: 16, color: watchface.isPaid ? Colors.orange : Colors.green),
                                            const SizedBox(width: 4),
                                            Text(
                                              watchface.isPaid ? 'Paid' : 'Free',
                                              style: TextStyle(color: watchface.isPaid ? Colors.orange : Colors.green, fontWeight: FontWeight.w500),
                                            ),
                                          ],
                                        ),
                                        if (watchface.permissions.isNotEmpty) ...[
                                          const SizedBox(height: 12),
                                          const Text('Required permissions:', style: TextStyle(fontWeight: FontWeight.w600, fontSize: 14)),
                                          const SizedBox(height: 4),
                                          ...watchface.permissions.map(
                                            (permission) => Padding(
                                              padding: const EdgeInsets.only(left: 8, top: 2),
                                              child: Row(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: [
                                                  const Text('• ', style: TextStyle(fontSize: 12)),
                                                  Expanded(child: Text(permission.description, style: const TextStyle(fontSize: 12))),
                                                ],
                                              ),
                                            ),
                                          ),
                                        ],
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        );
                      },
                    ),
            ),
          ],
        ),
      ),
    );
  }
}
