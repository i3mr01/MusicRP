using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace i3mr0_MusicRP {
    /// <summary>
    /// Performance monitoring and optimization for i3mr0 MusicRP
    /// </summary>
    public class PerformanceMonitor {
        private Logger? logger;
        private Timer performanceTimer;
        private Dictionary<string, PerformanceMetric> metrics;
        private List<PerformanceAlert> alerts;
        private bool isMonitoring = false;

        public PerformanceMonitor(i3mr0_MusicRP.Logger? logger = null) {
            this.logger = logger;
            this.metrics = new Dictionary<string, PerformanceMetric>();
            this.alerts = new List<PerformanceAlert>();
            this.performanceTimer = new Timer(30000); // Check every 30 seconds
            this.performanceTimer.Elapsed += OnPerformanceCheck;
        }

        public void StartMonitoring() {
            if (isMonitoring) return;
            
            isMonitoring = true;
            performanceTimer.Start();
            logger?.Log("[PerformanceMonitor] Started performance monitoring");
        }

        public void StopMonitoring() {
            if (!isMonitoring) return;
            
            isMonitoring = false;
            performanceTimer.Stop();
            logger?.Log("[PerformanceMonitor] Stopped performance monitoring");
        }

        public void RecordMetric(string name, double value, string unit = "") {
            if (!metrics.ContainsKey(name)) {
                metrics[name] = new PerformanceMetric {
                    Name = name,
                    Unit = unit,
                    Values = new List<double>(),
                    Timestamp = DateTime.UtcNow
                };
            }

            var metric = metrics[name];
            metric.Values.Add(value);
            metric.LastValue = value;
            metric.Timestamp = DateTime.UtcNow;

            // Keep only last 100 values to prevent memory issues
            if (metric.Values.Count > 100) {
                metric.Values.RemoveAt(0);
            }

            // Check for performance alerts
            CheckForAlerts(name, value);
        }

        public PerformanceReport GenerateReport() {
            var report = new PerformanceReport {
                GeneratedAt = DateTime.UtcNow,
                Metrics = new List<PerformanceMetric>(metrics.Values),
                Alerts = new List<PerformanceAlert>(alerts),
                SystemInfo = GetSystemInfo()
            };

            return report;
        }

        public void OptimizePerformance() {
            logger?.Log("[PerformanceMonitor] Starting performance optimization...");

            // Clear old cache entries
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Clear old alerts
            alerts.RemoveAll(a => (DateTime.UtcNow - a.Timestamp).TotalHours > 1);

            logger?.Log("[PerformanceMonitor] Performance optimization completed");
        }

        private void OnPerformanceCheck(object? sender, ElapsedEventArgs e) {
            try {
                // Monitor memory usage
                var process = Process.GetCurrentProcess();
                var memoryMB = process.WorkingSet64 / 1024 / 1024;
                RecordMetric("MemoryUsage", memoryMB, "MB");

                // Monitor CPU usage (simplified)
                var cpuUsage = GetCpuUsage();
                RecordMetric("CpuUsage", cpuUsage, "%");

                // Monitor response times
                RecordMetric("ResponseTime", GetAverageResponseTime(), "ms");

                // Check for performance issues
                CheckPerformanceIssues();

            } catch (Exception ex) {
                logger?.Log($"[PerformanceMonitor] Error during performance check: {ex.Message}");
            }
        }

        private void CheckForAlerts(string metricName, double value) {
            var alertThresholds = new Dictionary<string, double> {
                { "MemoryUsage", 500 }, // 500MB
                { "CpuUsage", 80 },     // 80%
                { "ResponseTime", 5000 } // 5 seconds
            };

            if (alertThresholds.ContainsKey(metricName) && value > alertThresholds[metricName]) {
                var alert = new PerformanceAlert {
                    MetricName = metricName,
                    Value = value,
                    Threshold = alertThresholds[metricName],
                    Timestamp = DateTime.UtcNow,
                    Severity = value > alertThresholds[metricName] * 1.5 ? AlertSeverity.High : AlertSeverity.Medium
                };

                alerts.Add(alert);
                logger?.Log($"[PerformanceMonitor] ALERT: {metricName} = {value} (threshold: {alertThresholds[metricName]})");
            }
        }

        private void CheckPerformanceIssues() {
            // Check for memory leaks
            var memoryMetric = metrics.GetValueOrDefault("MemoryUsage");
            if (memoryMetric != null && memoryMetric.Values.Count > 10) {
                var recentValues = memoryMetric.Values.TakeLast(10).ToList();
                var isIncreasing = recentValues.Zip(recentValues.Skip(1), (a, b) => b > a).All(x => x);
                
                if (isIncreasing) {
                    var alert = new PerformanceAlert {
                        MetricName = "MemoryLeak",
                        Value = recentValues.Last(),
                        Threshold = 0,
                        Timestamp = DateTime.UtcNow,
                        Severity = AlertSeverity.High
                    };
                    alerts.Add(alert);
                    logger?.Log("[PerformanceMonitor] ALERT: Potential memory leak detected");
                }
            }
        }

        private double GetCpuUsage() {
            try {
                var process = Process.GetCurrentProcess();
                var startTime = DateTime.UtcNow;
                var startCpuUsage = process.TotalProcessorTime;
                
                System.Threading.Thread.Sleep(100);
                
                var endTime = DateTime.UtcNow;
                var endCpuUsage = process.TotalProcessorTime;
                
                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                
                return cpuUsageTotal * 100;
            } catch {
                return 0;
            }
        }

        private double GetAverageResponseTime() {
            // This would measure actual response times from API calls
            // For now, return a simulated value
            return new Random().Next(100, 1000);
        }

        private SystemInfo GetSystemInfo() {
            return new SystemInfo {
                OsVersion = Environment.OSVersion.ToString(),
                ProcessorCount = Environment.ProcessorCount,
                TotalMemory = GC.GetTotalMemory(false),
                DotNetVersion = Environment.Version.ToString(),
                MachineName = Environment.MachineName
            };
        }

        public void Dispose() {
            StopMonitoring();
            performanceTimer?.Dispose();
        }
    }

    public class PerformanceMetric {
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public List<double> Values { get; set; } = new List<double>();
        public double LastValue { get; set; }
        public DateTime Timestamp { get; set; }
        public double Average => Values.Count > 0 ? Values.Average() : 0;
        public double Max => Values.Count > 0 ? Values.Max() : 0;
        public double Min => Values.Count > 0 ? Values.Min() : 0;
    }

    public class PerformanceAlert {
        public string MetricName { get; set; } = "";
        public double Value { get; set; }
        public double Threshold { get; set; }
        public DateTime Timestamp { get; set; }
        public AlertSeverity Severity { get; set; }
    }

    public enum AlertSeverity {
        Low,
        Medium,
        High,
        Critical
    }

    public class PerformanceReport {
        public DateTime GeneratedAt { get; set; }
        public List<PerformanceMetric> Metrics { get; set; } = new List<PerformanceMetric>();
        public List<PerformanceAlert> Alerts { get; set; } = new List<PerformanceAlert>();
        public SystemInfo SystemInfo { get; set; } = new SystemInfo();
    }

    public class SystemInfo {
        public string OsVersion { get; set; } = "";
        public int ProcessorCount { get; set; }
        public long TotalMemory { get; set; }
        public string DotNetVersion { get; set; } = "";
        public string MachineName { get; set; } = "";
    }
}
