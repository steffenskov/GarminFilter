import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import 'models/garmin_device.dart';
import 'models/app_view_model.dart';
import 'models/app_query_model.dart';
import 'models/permission.dart';
import 'services/garmin_api_service.dart';
import 'services/preferences_service.dart';

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
  List<GarminDevice> _devices = [];
  GarminDevice? _selectedDevice;
  String? _selectedOrderBy;
  bool _isLoading = false;
  String? _error;

  bool _isLoadingOrderBy = false;
  String? _orderByError;

  List<AppViewModel> _watchfaces = [];
  bool _isLoadingWatchfaces = false;
  String? _watchfaceError;
  int _currentPageIndex = 0;
  bool _hasMoreData = true;
  final ScrollController _scrollController = ScrollController();
  bool? _paid = null;
  List<Permission> _availablePermissions = [];
  List<String> _selectedExcludePermissions = [];
  List<String> _orderByOptions = [];
  bool _isLoadingPermissions = false;
  String? _permissionsError;

  @override
  void initState() {
    super.initState();
    _loadDevices();
    _loadOrderByOptions();
    _loadPermissions();
    _loadPreferences();
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

  Future<void> _loadOrderByOptions() async {
    setState(() {
      _isLoadingOrderBy = true;
      _orderByError = null;
    });

    try {
      final orderByOptions = await GarminApiService.getOrderByOptions();
      setState(() {
        _orderByOptions = orderByOptions;
        _isLoadingOrderBy = false;
      });

      // Load selected orderBy after orderBy options are loaded
      final selectedOrderBy = await PreferencesService.loadSelectedOrderBy(orderByOptions);
      if (selectedOrderBy != null) {
        setState(() {
          _selectedOrderBy = selectedOrderBy;
        });
      } else {
        setState(() {
          _selectedOrderBy = orderByOptions.first;
        });
      }
    } catch (e) {
      setState(() {
        _orderByError = e.toString();
        _isLoadingOrderBy = false;
      });
    }
  }

  Future<void> _loadDevices() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final devices = await GarminApiService.getDevices();
      setState(() {
        _devices = devices;
        _isLoading = false;
      });

      // Load selected device after devices are loaded
      final selectedDevice = await PreferencesService.loadSelectedDevice(devices);
      if (selectedDevice != null) {
        setState(() {
          _selectedDevice = selectedDevice;
        });
        // Trigger search with saved preferences
        _searchWatchfaces(selectedDevice, resetPagination: true);
      }
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _loadPermissions() async {
    setState(() {
      _isLoadingPermissions = true;
      _permissionsError = null;
    });

    try {
      final permissions = await GarminApiService.getPermissions();
      setState(() {
        _availablePermissions = permissions;
        _isLoadingPermissions = false;
      });
    } catch (e) {
      setState(() {
        _permissionsError = e.toString();
        _isLoadingPermissions = false;
      });
    }
  }

  Future<void> _loadPreferences() async {
    // Load include paid preference
    final includePaid = await PreferencesService.loadPaid();
    setState(() {
      _paid = includePaid;
    });

    // Load excluded permissions
    final excludedPermissions = await PreferencesService.loadExcludedPermissions();
    setState(() {
      _selectedExcludePermissions = excludedPermissions;
    });
  }

  Future<void> _searchWatchfaces(GarminDevice device, {bool resetPagination = true}) async {
    if (_selectedOrderBy == null) {
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
      final query = AppQueryModel(excludePermissions: _selectedExcludePermissions, paid: _paid, pageIndex: _currentPageIndex, pageSize: 30, orderBy: _selectedOrderBy!);

      final watchfaces = await GarminApiService.getWatchfaces(device.id, query);

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
      await _searchWatchfaces(_selectedDevice!, resetPagination: false);
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
                    if (_isLoading)
                      const Center(child: CircularProgressIndicator())
                    else if (_error != null)
                      Column(
                        children: [
                          Text('Error: $_error', style: TextStyle(color: Colors.red[700])),
                          const SizedBox(height: 8),
                          ElevatedButton(onPressed: _loadDevices, child: const Text('Retry')),
                        ],
                      )
                    else
                      Row(
                        children: [
                          SizedBox(width: 150, child: Text("Choose a device")),
                          Expanded(
                            child: Autocomplete<GarminDevice>(
                              optionsBuilder: (TextEditingValue textEditingValue) {
                                if (textEditingValue.text.isEmpty) {
                                  return _devices;
                                }
                                return _devices.where((device) => device.name.toLowerCase().contains(textEditingValue.text.toLowerCase()));
                              },
                              displayStringForOption: (GarminDevice device) => device.name,
                              onSelected: (GarminDevice device) async {
                                setState(() {
                                  _selectedDevice = device;
                                });
                                await PreferencesService.saveSelectedDevice(device.id);
                                _searchWatchfaces(device, resetPagination: true);
                              },
                              fieldViewBuilder: (context, textEditingController, focusNode, onFieldSubmitted) {
                                if (_selectedDevice != null) {
                                  textEditingController.text = _selectedDevice!.name;
                                }
                                return TextFormField(
                                  controller: textEditingController,
                                  focusNode: focusNode,
                                  decoration: InputDecoration(
                                    labelText: 'Choose a device',
                                    hintText: 'Type to search devices...',
                                    border: const OutlineInputBorder(),
                                    suffixIcon: _selectedDevice != null
                                        ? IconButton(
                                            icon: const Icon(Icons.clear),
                                            onPressed: () async {
                                              textEditingController.clear();
                                              setState(() {
                                                _selectedDevice = null;
                                              });
                                              await PreferencesService.saveSelectedDevice(null);
                                            },
                                          )
                                        : null,
                                  ),
                                  onChanged: (value) {
                                    // Clear selection if text doesn't match any device
                                    if (_selectedDevice != null && _selectedDevice!.name != value) {
                                      setState(() {
                                        _selectedDevice = null;
                                      });
                                    }
                                  },
                                );
                              },
                            ),
                          ),
                        ],
                      ),
                    SizedBox(height: 8),
                    Row(
                      children: [
                        SizedBox(width: 150, child: Text("Pricing model")),
                        DropdownMenu<bool?>(
                          dropdownMenuEntries: const [
                            DropdownMenuEntry(label: "Both free and paid", value: null),
                            DropdownMenuEntry(label: "Only free", value: false),
                            DropdownMenuEntry(label: "Only paid", value: true),
                          ],
                          initialSelection: _paid,
                          onSelected: (bool? value) async {
                            setState(() {
                              _paid = value;
                            });
                            await PreferencesService.savePaid(_paid);
                            // Reset search when checkbox changes
                            if (_selectedDevice != null) {
                              _searchWatchfaces(_selectedDevice!, resetPagination: true);
                            }
                          },
                        ),
                      ],
                    ),
                    SizedBox(height: 8),

                    if (_isLoadingPermissions)
                      const Center(child: CircularProgressIndicator())
                    else if (_permissionsError != null)
                      Column(
                        children: [
                          Text('Error loading permissions: $_permissionsError', style: TextStyle(color: Colors.red[700])),
                          const SizedBox(height: 8),
                          ElevatedButton(onPressed: _loadPermissions, child: const Text('Retry')),
                        ],
                      )
                    else if (_availablePermissions.isEmpty)
                      const Text('No permissions available')
                    else
                      ExpansionTile(
                        title: Text("Excluded permissions: " + _selectedExcludePermissions.length.toString()),
                        tilePadding: EdgeInsets.all(0.0),
                        children: _availablePermissions
                            .map(
                              (permission) => CheckboxListTile(
                                title: Text(permission.description),
                                value: _selectedExcludePermissions.contains(permission.permission),
                                onChanged: (bool? value) async {
                                  setState(() {
                                    if (value == true) {
                                      _selectedExcludePermissions.add(permission.permission);
                                    } else {
                                      _selectedExcludePermissions.remove(permission.permission);
                                    }
                                  });
                                  await PreferencesService.saveExcludedPermissions(_selectedExcludePermissions);
                                  // Reset search when permissions change
                                  if (_selectedDevice != null) {
                                    _searchWatchfaces(_selectedDevice!, resetPagination: true);
                                  }
                                },
                              ),
                            )
                            .toList(),
                      ),
                    const SizedBox(height: 8),
                    if (_isLoadingOrderBy)
                      const Center(child: CircularProgressIndicator())
                    else if (_orderByError != null)
                      Column(
                        children: [
                          Text('Error: $_orderByError', style: TextStyle(color: Colors.red[700])),
                          const SizedBox(height: 8),
                          ElevatedButton(onPressed: _loadOrderByOptions, child: const Text('Retry')),
                        ],
                      )
                    else
                      Row(
                        children: [
                          SizedBox(width: 150, child: Text("Order by")),
                          DropdownMenu<String>(
                            dropdownMenuEntries: _orderByOptions.map((item) => DropdownMenuEntry(label: item, value: item)).toList(),
                            initialSelection: _selectedOrderBy,
                            onSelected: (String? value) async {
                              setState(() {
                                _selectedOrderBy = value;
                              });
                              await PreferencesService.saveSelectedOrderBy(value);
                              if (_selectedDevice != null) {
                                _searchWatchfaces(_selectedDevice!, resetPagination: true);
                              }
                            },
                          ),
                        ],
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
                          ElevatedButton(onPressed: () => _searchWatchfaces(_selectedDevice!), child: const Text('Retry')),
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
                                      width: 120,
                                      height: 120,
                                      fit: BoxFit.cover,
                                      errorBuilder: (context, error, stackTrace) {
                                        return Container(width: 120, height: 120, color: Colors.grey[300], child: const Icon(Icons.image_not_supported));
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
