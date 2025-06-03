(async () => {
    let orderData, data, monthlyData;
    const initMonthObj = {
        1: {
            total: 0,
            count: 0,
        },
        2: {
            total: 0,
            count: 0,
        },
        3: {
            total: 0,
            count: 0,
        },
        4: {
            total: 0,
            count: 0,
        },
        5: {
            total: 0,
            count: 0,
        },
        6: {
            total: 0,
            count: 0,
        },
        7: {
            total: 0,
            count: 0,
        },
        8: {
            total: 0,
            count: 0,
        },
        9: {
            total: 0,
            count: 0,
        },
        10: {
            total: 0,
            count: 0,
        },
        11: {
            total: 0,
            count: 0,
        },
        12: {
            total: 0,
            count: 0,
        },
    };

    const initWeekObj = {
        1: { total: 0, count: 0 },
        2: { total: 0, count: 0 },
        3: { total: 0, count: 0 },
        4: { total: 0, count: 0 },
        5: { total: 0, count: 0 },
        6: { total: 0, count: 0 },
        7: { total: 0, count: 0 },
        8: { total: 0, count: 0 },
        9: { total: 0, count: 0 },
        10: { total: 0, count: 0 },
        11: { total: 0, count: 0 },
        12: { total: 0, count: 0 },
        13: { total: 0, count: 0 },
        14: { total: 0, count: 0 },
        15: { total: 0, count: 0 },
        16: { total: 0, count: 0 },
        17: { total: 0, count: 0 },
        18: { total: 0, count: 0 },
        19: { total: 0, count: 0 },
        20: { total: 0, count: 0 },
        21: { total: 0, count: 0 },
        22: { total: 0, count: 0 },
        23: { total: 0, count: 0 },
        24: { total: 0, count: 0 },
        25: { total: 0, count: 0 },
        26: { total: 0, count: 0 },
        27: { total: 0, count: 0 },
        28: { total: 0, count: 0 },
        29: { total: 0, count: 0 },
        30: { total: 0, count: 0 },
        31: { total: 0, count: 0 },
        32: { total: 0, count: 0 },
        33: { total: 0, count: 0 },
        34: { total: 0, count: 0 },
        35: { total: 0, count: 0 },
        36: { total: 0, count: 0 },
        37: { total: 0, count: 0 },
        38: { total: 0, count: 0 },
        39: { total: 0, count: 0 },
        40: { total: 0, count: 0 },
        41: { total: 0, count: 0 },
        42: { total: 0, count: 0 },
        43: { total: 0, count: 0 },
        44: { total: 0, count: 0 },
        45: { total: 0, count: 0 },
        46: { total: 0, count: 0 },
        47: { total: 0, count: 0 },
        48: { total: 0, count: 0 },
        49: { total: 0, count: 0 },
        50: { total: 0, count: 0 },
        51: { total: 0, count: 0 },
        52: { total: 0, count: 0 }
    }

    function getWeekNumber(date) {
        const currentDate = new Date(date);
        currentDate.setHours(0, 0, 0, 0); // Set time to midnight for consistency

        // Get the first day of the year
        const firstDayOfYear = new Date(currentDate.getFullYear(), 0, 1);
        const dayOfYear = ((currentDate - firstDayOfYear) / 86400000) + 1; // Get the number of days since Jan 1

        // ISO Week Number formula
        const weekNumber = Math.ceil(dayOfYear / 7);

        return weekNumber;
    }

    const response = await $.ajax({
        url: "/admin/order/getall",
        method: "GET",
        contentType: "application/json"
    });

    data = response.data;

    monthlyData = data?.reduce((acc, item) => {
        const month = item.orderDate.substring(5, 7);
        const monthNumber = Number(month);
        acc[monthNumber].total += item.orderTotal;
        acc[monthNumber].count++;
        return acc;
    }, initMonthObj);

    const weeklyData = data.reduce((acc, item) => {
        const week = getWeekNumber(item.orderDate.substring(0, 7));
        // console.log(week);
        acc[week].total += item.orderTotal;
        acc[week].count++;
        return acc;
    }, initWeekObj);

    const resultMonthlyCount = [
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
    ];
    const resultMonthlyRevenue = [
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        [],
    ];

    Object.entries(weeklyData).forEach(([key, value]) => {
        resultMonthlyCount[Math.floor((key - 1) / 4)].push(value.count);
        resultMonthlyRevenue[Math.floor((key - 1) / 4)].push(value.total);
    });

    
    orderData = {
        '2025': {
            months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            orderCounts: Object.entries(monthlyData ?? initArr).map(([key, value]) => value.count),
            revenue: Object.entries(monthlyData ?? initArr).map(([key, value]) => value.total),
            categories: {
                'Electronics': 823,
                'Clothing': 651,
                'Home & Kitchen': 542,
                'Books': 389,
                'Others': 159
            },
            // Weekly data - 52 weeks in a year, organized by month
            weeklyData: resultMonthlyCount,
            // Weekly revenue data
            weeklyRevenue: resultMonthlyRevenue,
            // Daily data for each week
            dailyData: Array.from({ length: 52 }, (_, weekIndex) => {
                // Base value that increases throughout the year
                const baseValue = 15 + Math.floor(weekIndex / 4) * 5;

                // Generate daily values with a pattern (weekends lower, midweek higher)
                return [
                    baseValue + Math.floor(Math.random() * 5),      // Monday
                    baseValue + 2 + Math.floor(Math.random() * 5),  // Tuesday
                    baseValue + 5 + Math.floor(Math.random() * 5),  // Wednesday (peak)
                    baseValue + 3 + Math.floor(Math.random() * 5),  // Thursday
                    baseValue + 4 + Math.floor(Math.random() * 5),  // Friday
                    baseValue - 5 + Math.floor(Math.random() * 3),  // Saturday (lower)
                    baseValue - 7 + Math.floor(Math.random() * 3)   // Sunday (lowest)
                ];
            })
        },
        '2024': {
            months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            orderCounts: [120, 115, 125, 135, 148, 162, 175, 189, 201, 215, 228, 240],
            revenue: [12000, 11500, 12500, 13500, 14800, 16200, 17500, 18900, 20100, 21500, 22800, 24000],
            categories: {
                'Electronics': 689,
                'Clothing': 542,
                'Home & Kitchen': 456,
                'Books': 325,
                'Others': 141
            },
            // Weekly data - 52 weeks in a year, organized by month
            weeklyData: [
                // January (weeks 0-3)
                [28, 30, 31, 31],
                // February (weeks 4-7)
                [27, 28, 29, 31],
                // March (weeks 8-11)
                [30, 31, 32, 32],
                // April (weeks 12-15)
                [32, 33, 34, 36],
                // May (weeks 16-19)
                [35, 36, 38, 39],
                // June (weeks 20-23)
                [38, 40, 42, 42],
                // July (weeks 24-27)
                [42, 43, 44, 46],
                // August (weeks 28-31)
                [45, 47, 48, 49],
                // September (weeks 32-35)
                [48, 50, 51, 52],
                // October (weeks 36-39)
                [52, 53, 54, 56],
                // November (weeks 40-43)
                [55, 56, 58, 59],
                // December (weeks 44-47, 48-51)
                [58, 59, 61, 62]
            ],
            // Weekly revenue data
            weeklyRevenue: [
                // January (weeks 0-3)
                [2800, 3000, 3100, 3100],
                // February (weeks 4-7)
                [2700, 2800, 2900, 3100],
                // March (weeks 8-11)
                [3000, 3100, 3200, 3200],
                // April (weeks 12-15)
                [3200, 3300, 3400, 3600],
                // May (weeks 16-19)
                [3500, 3600, 3800, 3900],
                // June (weeks 20-23)
                [3800, 4000, 4200, 4200],
                // July (weeks 24-27)
                [4200, 4300, 4400, 4600],
                // August (weeks 28-31)
                [4500, 4700, 4800, 4900],
                // September (weeks 32-35)
                [4800, 5000, 5100, 5200],
                // October (weeks 36-39)
                [5200, 5300, 5400, 5600],
                // November (weeks 40-43)
                [5500, 5600, 5800, 5900],
                // December (weeks 44-47, 48-51)
                [5800, 5900, 6100, 6200]
            ],
            // Daily data for each week
            dailyData: Array.from({ length: 52 }, (_, weekIndex) => {
                // Base value that increases throughout the year
                const baseValue = 12 + Math.floor(weekIndex / 4) * 4;

                // Generate daily values with a pattern (weekends lower, midweek higher)
                return [
                    baseValue + Math.floor(Math.random() * 4),      // Monday
                    baseValue + 2 + Math.floor(Math.random() * 4),  // Tuesday
                    baseValue + 4 + Math.floor(Math.random() * 4),  // Wednesday (peak)
                    baseValue + 3 + Math.floor(Math.random() * 4),  // Thursday
                    baseValue + 3 + Math.floor(Math.random() * 4),  // Friday
                    baseValue - 4 + Math.floor(Math.random() * 3),  // Saturday (lower)
                    baseValue - 6 + Math.floor(Math.random() * 3)   // Sunday (lowest)
                ];
            })
        },
        '2023': {
            months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            orderCounts: [98, 102, 110, 118, 125, 135, 145, 155, 165, 175, 185, 195],
            revenue: [9800, 10200, 11000, 11800, 12500, 13500, 14500, 15500, 16500, 17500, 18500, 19500],
            categories: {
                'Electronics': 578,
                'Clothing': 456,
                'Home & Kitchen': 389,
                'Books': 275,
                'Others': 110
            },
            // Weekly data - 52 weeks in a year, organized by month
            weeklyData: [
                // January (weeks 0-3)
                [23, 24, 25, 26],
                // February (weeks 4-7)
                [24, 25, 26, 27],
                // March (weeks 8-11)
                [26, 27, 28, 29],
                // April (weeks 12-15)
                [28, 29, 30, 31],
                // May (weeks 16-19)
                [30, 31, 32, 32],
                // June (weeks 20-23)
                [32, 33, 34, 36],
                // July (weeks 24-27)
                [35, 36, 37, 37],
                // August (weeks 28-31)
                [37, 38, 39, 41],
                // September (weeks 32-35)
                [40, 41, 42, 42],
                // October (weeks 36-39)
                [42, 43, 44, 46],
                // November (weeks 40-43)
                [45, 46, 47, 47],
                // December (weeks 44-47, 48-51)
                [47, 48, 49, 51]
            ],
            // Weekly revenue data
            weeklyRevenue: [
                // January (weeks 0-3)
                [2300, 2400, 2500, 2600],
                // February (weeks 4-7)
                [2400, 2500, 2600, 2700],
                // March (weeks 8-11)
                [2600, 2700, 2800, 2900],
                // April (weeks 12-15)
                [2800, 2900, 3000, 3100],
                // May (weeks 16-19)
                [3000, 3100, 3200, 3200],
                // June (weeks 20-23)
                [3200, 3300, 3400, 3600],
                // July (weeks 24-27)
                [3500, 3600, 3700, 3700],
                // August (weeks 28-31)
                [3700, 3800, 3900, 4100],
                // September (weeks 32-35)
                [4000, 4100, 4200, 4200],
                // October (weeks 36-39)
                [4200, 4300, 4400, 4600],
                // November (weeks 40-43)
                [4500, 4600, 4700, 4700],
                // December (weeks 44-47, 48-51)
                [4700, 4800, 4900, 5100]
            ],
            // Daily data for each week
            dailyData: Array.from({ length: 52 }, (_, weekIndex) => {
                // Base value that increases throughout the year
                const baseValue = 10 + Math.floor(weekIndex / 4) * 3;

                // Generate daily values with a pattern (weekends lower, midweek higher)
                return [
                    baseValue + Math.floor(Math.random() * 3),      // Monday
                    baseValue + 1 + Math.floor(Math.random() * 3),  // Tuesday
                    baseValue + 3 + Math.floor(Math.random() * 3),  // Wednesday (peak)
                    baseValue + 2 + Math.floor(Math.random() * 3),  // Thursday
                    baseValue + 2 + Math.floor(Math.random() * 3),  // Friday
                    baseValue - 3 + Math.floor(Math.random() * 2),  // Saturday (lower)
                    baseValue - 5 + Math.floor(Math.random() * 2)   // Sunday (lowest)
                ];
            })
        }
    };

    // Chart instances
    let yearlyTrendsChart;
    let orderDistributionChart;
    let monthlyTrendsChart;
    let dailyTrendsChart;
    let dayOfWeekChart;

    // Current state
    let currentYear = '2025';
    let currentMonth = 0; // 0-indexed, January
    let currentWeek = 0; // 0-indexed, Week 1
    let currentDataType = 'count';

    // Colors for charts
    const colors = {
        primary: '#4f46e5',
        secondary: '#10b981',
        accent: '#f59e0b',
        danger: '#ef4444',
        info: '#3b82f6',
        categories: [
            'rgba(79, 70, 229, 0.7)',
            'rgba(16, 185, 129, 0.7)',
            'rgba(245, 158, 11, 0.7)',
            'rgba(239, 68, 68, 0.7)',
            'rgba(59, 130, 246, 0.7)',
            'rgba(124, 58, 237, 0.7)',
            'rgba(236, 72, 153, 0.7)',
            'rgba(234, 88, 12, 0.7)',
            'rgba(22, 163, 74, 0.7)',
            'rgba(2, 132, 199, 0.7)',
            'rgba(71, 85, 105, 0.7)',
            'rgba(190, 18, 60, 0.7)'
        ],
        days: [
            'rgba(79, 70, 229, 0.7)',  // Monday
            'rgba(79, 70, 229, 0.75)', // Tuesday
            'rgba(79, 70, 229, 0.9)',  // Wednesday
            'rgba(79, 70, 229, 0.8)',  // Thursday
            'rgba(79, 70, 229, 0.85)', // Friday
            'rgba(79, 70, 229, 0.5)',  // Saturday
            'rgba(79, 70, 229, 0.4)'   // Sunday
        ]
    };

    // Day names
    const dayNames = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

    // Initialize dashboard when the page loads
    window.onload = function () {
        initYearlyCharts();
        updateYearlyStats();
        populateMonthlyTable();
        updateBreadcrumb();
    };

    // Change the selected year
    window.changeYear = () => {
        currentYear = document.getElementById('year').value;

        // Reset to yearly view
        showYearlyView();

        // Update charts and stats
        updateYearlyCharts();
        updateYearlyStats();
        populateMonthlyTable();
        updateBreadcrumb();
    }

    // Initialize yearly charts
    window.initYearlyCharts = () => {
        initYearlyTrendsChart();
        initOrderDistributionChart();
    }

    // Initialize the yearly trends chart
    window.initYearlyTrendsChart = () => {
        const ctx = document.getElementById('yearlyTrendsChart').getContext('2d');

        yearlyTrendsChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: orderData[currentYear].monthsShort,
                datasets: [{
                    label: 'Order Count',
                    data: orderData[currentYear].orderCounts,
                    borderColor: colors.primary,
                    backgroundColor: 'rgba(79, 70, 229, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            drawBorder: false
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Initialize the order distribution chart - now showing orders by month
    window.initOrderDistributionChart = () => {
        const ctx = document.getElementById('orderDistributionChart').getContext('2d');

        // Use monthly data instead of categories
        const months = orderData[currentYear].monthsShort;
        const values = orderData[currentYear].orderCounts;

        orderDistributionChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: months,
                datasets: [{
                    label: 'Orders',
                    data: values,
                    backgroundColor: colors.categories,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            drawBorder: false
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Initialize monthly charts
    window.initMonthlyCharts = () => {
        const ctx = document.getElementById('monthlyTrendsChart').getContext('2d');

        // Get weekly data for the selected month
        const weeklyData = orderData[currentYear].weeklyData[currentMonth];
        const weekLabels = Array.from({ length: weeklyData.length }, (_, i) => `Week ${i + 1}`);

        monthlyTrendsChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: weekLabels,
                datasets: [{
                    label: 'Weekly Orders',
                    data: weeklyData,
                    borderColor: colors.primary,
                    backgroundColor: 'rgba(79, 70, 229, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            drawBorder: false
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Initialize weekly charts
    window.initWeeklyCharts = () => {
        initDailyTrendsChart();
        initDayOfWeekChart();
    }

    // Initialize the daily trends chart
    window.initDailyTrendsChart = () => {
        const ctx = document.getElementById('dailyTrendsChart').getContext('2d');

        // Calculate the actual week index based on month and week
        const weekIndex = getWeekIndex(currentMonth, currentWeek);

        dailyTrendsChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dayNames,
                datasets: [{
                    label: 'Daily Orders',
                    data: orderData[currentYear].dailyData[weekIndex],
                    borderColor: colors.primary,
                    backgroundColor: 'rgba(79, 70, 229, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            drawBorder: false
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Initialize the day of week analysis chart
    window.initDayOfWeekChart = () => {
        const ctx = document.getElementById('dayOfWeekChart').getContext('2d');

        // Calculate the actual week index based on month and week
        const weekIndex = getWeekIndex(currentMonth, currentWeek);

        dayOfWeekChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: dayNames,
                datasets: [{
                    label: 'Orders by Day of Week',
                    data: orderData[currentYear].dailyData[weekIndex],
                    backgroundColor: colors.days,
                    borderColor: 'rgba(79, 70, 229, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            drawBorder: false
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Update yearly charts
    window.updateYearlyCharts = () => {
        // Update trends chart
        yearlyTrendsChart.data.labels = orderData[currentYear].monthsShort;
        if (currentDataType === 'count') {
            yearlyTrendsChart.data.datasets[0].data = orderData[currentYear].orderCounts;
            yearlyTrendsChart.data.datasets[0].label = 'Order Count';
        } else {
            yearlyTrendsChart.data.datasets[0].data = orderData[currentYear].revenue;
            yearlyTrendsChart.data.datasets[0].label = 'Revenue ($)';
        }
        yearlyTrendsChart.update();

        // Update distribution chart - now showing orders by month
        const months = orderData[currentYear].monthsShort;
        const values = orderData[currentYear].orderCounts;

        orderDistributionChart.data.labels = months;
        orderDistributionChart.data.datasets[0].data = values;
        orderDistributionChart.update();
    }

    // Update monthly charts
    window.updateMonthlyCharts = () => {
        // Get weekly data for the selected month
        const weeklyData = orderData[currentYear].weeklyData[currentMonth];
        const weekLabels = Array.from({ length: weeklyData.length }, (_, i) => `Week ${i + 1}`);

        monthlyTrendsChart.data.labels = weekLabels;
        monthlyTrendsChart.data.datasets[0].data = weeklyData;
        monthlyTrendsChart.update();
    }

    // Update weekly charts
    window.updateWeeklyCharts = () => {
        // Calculate the actual week index based on month and week
        const weekIndex = getWeekIndex(currentMonth, currentWeek);

        // Update daily trends chart
        dailyTrendsChart.data.datasets[0].data = orderData[currentYear].dailyData[weekIndex];
        dailyTrendsChart.update();

        // Update day of week chart
        dayOfWeekChart.data.datasets[0].data = orderData[currentYear].dailyData[weekIndex];
        dayOfWeekChart.update();
    }

    // Toggle between showing order count and revenue data
    window.toggleDataType = (type) => {
        currentDataType = type;

        if (type === 'count') {
            yearlyTrendsChart.data.datasets[0].data = orderData[currentYear].orderCounts;
            yearlyTrendsChart.data.datasets[0].label = 'Order Count';
            yearlyTrendsChart.data.datasets[0].borderColor = colors.primary;
            yearlyTrendsChart.data.datasets[0].backgroundColor = 'rgba(79, 70, 229, 0.1)';
            document.getElementById('btnShowRevenue').classList.toggle('secondary');
            document.getElementById('btnShowCount').classList.toggle('secondary');
        } else {
            yearlyTrendsChart.data.datasets[0].data = orderData[currentYear].revenue;
            yearlyTrendsChart.data.datasets[0].label = 'Revenue ($)';
            yearlyTrendsChart.data.datasets[0].borderColor = colors.secondary;
            yearlyTrendsChart.data.datasets[0].backgroundColor = 'rgba(16, 185, 129, 0.1)';
            document.getElementById('btnShowRevenue').classList.toggle('secondary');
            document.getElementById('btnShowCount').classList.toggle('secondary');
        }

        yearlyTrendsChart.update();
    }

    // Update yearly statistics
    window.updateYearlyStats = () => {
        const data = orderData[currentYear];

        // Calculate total orders
        const totalOrders = data.orderCounts.reduce((sum, count) => sum + count, 0);
        document.getElementById('total-orders-yearly').textContent = totalOrders.toLocaleString();

        // Calculate average orders per month
        const avgOrders = Math.round(totalOrders / 12);
        document.getElementById('avg-orders-monthly').textContent = avgOrders.toLocaleString();

        // Calculate total revenue
        const totalRevenue = data.revenue.reduce((sum, rev) => sum + rev, 0);
        document.getElementById('total-revenue-yearly').textContent = '$' + totalRevenue.toLocaleString();

        // Calculate YoY growth rate (if previous year data exists)
        let growthRate = 0;
        const prevYear = (parseInt(currentYear) - 1).toString();

        if (orderData[prevYear]) {
            const prevTotalOrders = orderData[prevYear].orderCounts.reduce((sum, count) => sum + count, 0);
            growthRate = Math.round(((totalOrders - prevTotalOrders) / prevTotalOrders) * 100);
        }

        const growthElement = document.getElementById('growth-rate-yearly');
        growthElement.textContent = growthRate + '%';

        // Set color based on growth rate
        if (growthRate > 0) {
            growthElement.style.color = colors.secondary;
        } else if (growthRate < 0) {
            growthElement.style.color = colors.danger;
        } else {
            growthElement.style.color = colors.primary;
        }
    }

    // Update monthly statistics
    window.updateMonthlyStats = () => {
        const data = orderData[currentYear];
        const monthlyOrderCount = data.orderCounts[currentMonth];
        const monthlyRevenue = data.revenue[currentMonth];
        const weeklyData = data.weeklyData[currentMonth];

        // Total orders for the month
        document.getElementById('total-orders-monthly').textContent = monthlyOrderCount.toLocaleString();

        // Average orders per week
        const avgWeeklyOrders = Math.round(monthlyOrderCount / weeklyData.length);
        document.getElementById('avg-orders-weekly').textContent = avgWeeklyOrders.toLocaleString();

        // Monthly revenue
        document.getElementById('total-revenue-monthly').textContent = '$' + monthlyRevenue.toLocaleString();

        // Month-over-month growth
        let monthlyGrowth = 0;
        if (currentMonth > 0) {
            const prevMonthOrders = data.orderCounts[currentMonth - 1];
            monthlyGrowth = Math.round(((monthlyOrderCount - prevMonthOrders) / prevMonthOrders) * 100);
        } else if (currentYear > 2021) {
            const prevYear = (parseInt(currentYear) - 1).toString();
            const prevDecOrders = orderData[prevYear].orderCounts[11]; // December of previous year
            monthlyGrowth = Math.round(((monthlyOrderCount - prevDecOrders) / prevDecOrders) * 100);
        }

        const growthElement = document.getElementById('growth-rate-monthly');
        growthElement.textContent = monthlyGrowth + '%';

        // Set color based on growth rate
        if (monthlyGrowth > 0) {
            growthElement.style.color = colors.secondary;
        } else if (monthlyGrowth < 0) {
            growthElement.style.color = colors.danger;
        } else {
            growthElement.style.color = colors.primary;
        }
    }

    // Update weekly statistics
    window.updateWeeklyStats = () => {
        // Calculate the actual week index based on month and week
        const weekIndex = getWeekIndex(currentMonth, currentWeek);

        const weeklyData = orderData[currentYear].dailyData[weekIndex];

        // Calculate total weekly orders
        const totalWeeklyOrders = weeklyData.reduce((sum, count) => sum + count, 0);
        document.getElementById('total-orders-weekly').textContent = totalWeeklyOrders.toLocaleString();

        // Calculate average orders per day
        const avgDailyOrders = Math.round(totalWeeklyOrders / 7);
        document.getElementById('avg-orders-daily').textContent = avgDailyOrders.toLocaleString();

        // Find peak order day
        const peakDayIndex = weeklyData.indexOf(Math.max(...weeklyData));
        document.getElementById('peak-day').textContent = dayNames[peakDayIndex];

        // Calculate week-over-week growth
        let weeklyGrowth = 0;
        if (weekIndex > 0) {
            const prevWeekData = orderData[currentYear].dailyData[weekIndex - 1];
            const prevWeekTotal = prevWeekData.reduce((sum, count) => sum + count, 0);
            weeklyGrowth = Math.round(((totalWeeklyOrders - prevWeekTotal) / prevWeekTotal) * 100);
        }

        const weeklyGrowthElement = document.getElementById('weekly-growth');
        weeklyGrowthElement.textContent = weeklyGrowth + '%';

        // Set color based on growth rate
        if (weeklyGrowth > 0) {
            weeklyGrowthElement.style.color = colors.secondary;
        } else if (weeklyGrowth < 0) {
            weeklyGrowthElement.style.color = colors.danger;
        } else {
            weeklyGrowthElement.style.color = colors.primary;
        }

        // Populate daily table
        populateDailyTable(weeklyData);
    }

    // Populate the monthly table - removed Avg. Order Value and MoM Growth columns
    window.populateMonthlyTable = () => {
        const tableBody = document.getElementById('monthly-table').getElementsByTagName('tbody')[0];
        tableBody.innerHTML = '';

        const data = orderData[currentYear];

        data.months.forEach((month, index) => {
            const orderCount = data.orderCounts[index];
            const revenue = data.revenue[index];

            const row = tableBody.insertRow();

            // Month name
            const monthCell = row.insertCell(0);
            monthCell.textContent = month;

            // Order count
            const ordersCell = row.insertCell(1);
            ordersCell.textContent = orderCount.toLocaleString();

            // Revenue
            const revenueCell = row.insertCell(2);
            revenueCell.textContent = '$' + revenue.toLocaleString();

            // Action button
            const actionCell = row.insertCell(3);
            const viewButton = document.createElement('button');
            viewButton.textContent = 'View Details';
            viewButton.className = 'secondary';
            viewButton.onclick = function () {
                viewMonthDetails(index);
            };
            actionCell.appendChild(viewButton);
        });
    }

    // Populate the weekly table - removed Avg. Order Value and WoW Growth columns
    window.populateWeeklyTable = () => {
        const tableBody = document.getElementById('weekly-table').getElementsByTagName('tbody')[0];
        tableBody.innerHTML = '';

        const weeklyData = orderData[currentYear].weeklyData[currentMonth];
        const weeklyRevenue = orderData[currentYear].weeklyRevenue[currentMonth];

        weeklyData.forEach((orderCount, index) => {
            const revenue = weeklyRevenue[index];

            const row = tableBody.insertRow();

            // Week number
            const weekCell = row.insertCell(0);
            weekCell.textContent = `Week ${index + 1}`;

            // Order count
            const ordersCell = row.insertCell(1);
            ordersCell.textContent = orderCount.toLocaleString();

            // Revenue
            const revenueCell = row.insertCell(2);
            revenueCell.textContent = '$' + revenue.toLocaleString();

            // Action button
            const actionCell = row.insertCell(3);
            const viewButton = document.createElement('button');
            viewButton.textContent = 'View Details';
            viewButton.className = 'secondary';
            viewButton.onclick = function () {
                viewWeekDetails(index);
            };
            actionCell.appendChild(viewButton);
        });
    }

    // Populate the daily table - removed % of Weekly Total and Compared to Avg. columns
    window.populateDailyTable = (dailyData) => {
        const tableBody = document.getElementById('daily-table').getElementsByTagName('tbody')[0];
        tableBody.innerHTML = '';

        dailyData.forEach((orderCount, index) => {
            const row = tableBody.insertRow();

            // Day name
            const dayCell = row.insertCell(0);
            dayCell.textContent = dayNames[index];

            // Order count
            const ordersCell = row.insertCell(1);
            ordersCell.textContent = orderCount.toLocaleString();
        });
    }

    // View month details
    window.viewMonthDetails = (monthIndex) => {
        currentMonth = monthIndex;

        // Update selected month display
        document.getElementById('selected-month').textContent = orderData[currentYear].months[monthIndex];

        // Initialize or update monthly charts
        if (!monthlyTrendsChart) {
            initMonthlyCharts();
        } else {
            updateMonthlyCharts();
        }

        // Update monthly stats
        updateMonthlyStats();

        // Populate weekly table
        populateWeeklyTable();

        // Show monthly view
        showMonthlyView();

        // Update breadcrumb
        updateBreadcrumb();
    }

    // View week details
    window.viewWeekDetails = (weekIndex) => {
        currentWeek = weekIndex;

        // Update selected week display
        document.getElementById('selected-week').textContent = `Week ${weekIndex + 1}`;

        // Initialize or update weekly charts
        if (!dailyTrendsChart || !dayOfWeekChart) {
            initWeeklyCharts();
        } else {
            updateWeeklyCharts();
        }

        // Update weekly stats
        updateWeeklyStats();

        // Show weekly view
        showWeeklyView();

        // Update breadcrumb
        updateBreadcrumb();
    }

    // Show yearly view
    window.showYearlyView = () => {
        document.getElementById('yearly-section').style.display = 'block';
        document.getElementById('monthly-section').style.display = 'none';
        document.getElementById('weekly-section').style.display = 'none';
    }

    // Show monthly view
    window.showMonthlyView = () => {
        document.getElementById('yearly-section').style.display = 'none';
        document.getElementById('monthly-section').style.display = 'block';
        document.getElementById('weekly-section').style.display = 'none';
    }

    // Show weekly view
    window.showWeeklyView = () => {
        document.getElementById('yearly-section').style.display = 'none';
        document.getElementById('monthly-section').style.display = 'none';
        document.getElementById('weekly-section').style.display = 'block';
    }

    // Back to yearly view
    window.backToYearly = () => {
        showYearlyView();
        updateBreadcrumb();
    }

    // Back to monthly view
    window.backToMonthly = () => {
        showMonthlyView();
        updateBreadcrumb();
    }

    // Update breadcrumb
    window.updateBreadcrumb = () => {
        // Year is always visible
        document.getElementById('year-crumb').textContent = currentYear;

        // Show/hide month
        if (document.getElementById('monthly-section').style.display === 'block' ||
            document.getElementById('weekly-section').style.display === 'block') {
            document.getElementById('month-separator').style.display = 'inline';
            document.getElementById('month-crumb').style.display = 'inline';
            document.getElementById('month-crumb').textContent = orderData[currentYear].months[currentMonth];

            // If on monthly view, month is active
            if (document.getElementById('monthly-section').style.display === 'block') {
                document.getElementById('year-crumb').className = 'breadcrumb-item';
                document.getElementById('month-crumb').className = 'breadcrumb-item active';
            } else {
                document.getElementById('month-crumb').className = 'breadcrumb-item';
            }
        } else {
            document.getElementById('month-separator').style.display = 'none';
            document.getElementById('month-crumb').style.display = 'none';
            document.getElementById('year-crumb').className = 'breadcrumb-item active';
        }

        // Show/hide week
        if (document.getElementById('weekly-section').style.display === 'block') {
            document.getElementById('week-separator').style.display = 'inline';
            document.getElementById('week-crumb').style.display = 'inline';
            document.getElementById('week-crumb').textContent = `Week ${currentWeek + 1}`;
            document.getElementById('week-crumb').className = 'breadcrumb-item active';
        } else {
            document.getElementById('week-separator').style.display = 'none';
            document.getElementById('week-crumb').style.display = 'none';
        }
    }

    // Helper function to get the actual week index based on month and week
    window.getWeekIndex = (month, week) => {
        // Calculate the week index based on month and week
        // Assuming each month has 4 weeks
        return month * 4 + week;
    }
})();