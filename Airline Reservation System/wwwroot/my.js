

$(function () {

    // Dashboard chart colors

    const body_styles = window.getComputedStyle(document.body);
    const colors = {
        primary: $.trim(body_styles.getPropertyValue('--bs-primary')),
        secondary: $.trim(body_styles.getPropertyValue('--bs-secondary')),
        info: $.trim(body_styles.getPropertyValue('--bs-info')),
        success: $.trim(body_styles.getPropertyValue('--bs-success')),
        danger: $.trim(body_styles.getPropertyValue('--bs-danger')),
        warning: $.trim(body_styles.getPropertyValue('--bs-warning')),
        light: $.trim(body_styles.getPropertyValue('--bs-light')),
        dark: $.trim(body_styles.getPropertyValue('--bs-dark')),
        blue: $.trim(body_styles.getPropertyValue('--bs-blue')),
        indigo: $.trim(body_styles.getPropertyValue('--bs-indigo')),
        purple: $.trim(body_styles.getPropertyValue('--bs-purple')),
        pink: $.trim(body_styles.getPropertyValue('--bs-pink')),
        red: $.trim(body_styles.getPropertyValue('--bs-red')),
        orange: $.trim(body_styles.getPropertyValue('--bs-orange')),
        yellow: $.trim(body_styles.getPropertyValue('--bs-yellow')),
        green: $.trim(body_styles.getPropertyValue('--bs-green')),
        teal: $.trim(body_styles.getPropertyValue('--bs-teal')),
        cyan: $.trim(body_styles.getPropertyValue('--bs-cyan')),
        chartTextColor: $('body').hasClass('dark') ? '#6c6c6c' : '#b8b8b8',
        chartBorderColor: $('body').hasClass('dark') ? '#444444' : '#ededed',
    };


    let chart;
    function salesChannels() {
        if ($('#sales-channels').length) {
            const options = {
                chart: {
                    height: 250,
                    type: 'donut',
                    offsetY: 0
                },
                plotOptions: {
                    pie: {
                        donut: {
                            size: '40%',
                        }
                    }
                },
                stroke: {
                    show: false,
                    width: 0
                },
                colors: [colors.orange, colors.cyan, colors.indigo],
                series: channel,
                labels: labels,
                legend: {
                    show: false
                }
            }

            chart = new ApexCharts(document.querySelector('#sales-channels'), options);
            chart.render()
        }
    }
    salesChannels();



    let Chart;
    function salesChart(Tickets, Flights) {
        if ($('#sales-chart').length) {
            // Hide the chart while updating
            // Show the chart after the new chart is rendered
            $('#sales-chart').hide();
            if (Chart) {

                Chart.destroy();
            }
                    const options = {
                        series: [
                            {
                                name: "Ticket Sold",
                                data: Tickets
                            },
                            {
                                name: 'Flight Depatures',
                                data: Flights
                            }
                        ],
                        theme: {
                            mode: $('body').hasClass('dark') ? 'dark' : 'light',
                        },
                        chart: {
                            height: 350,
                            type: 'line',
                            foreColor: colors.chartTextColor,
                            zoom: {
                                enabled: false
                            },
                            toolbar: {
                                show: false
                            }
                        },
                        dataLabels: {
                            enabled: false
                        },
                        colors: [colors.primary, colors.success],
                        stroke: {
                            width: 4,
                            curve: 'smooth',
                        },
                        legend: {
                            show: false
                        },
                        markers: {
                            size: 0,
                            hover: {
                                sizeOffset: 6
                            }
                        },
                        xaxis: {
                            categories: ['Jan', 'Feb', 'Mars', 'April', 'May', 'June', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                        },
                        tooltip: {

                            y: [
                                {
                                    title: {
                                        formatter: function (val) {
                                            return val
                                        }
                                    }
                                },
                                {
                                    title: {
                                        formatter: function (val) {
                                            return val
                                        }
                                    }
                                },
                                {
                                    title: {
                                        formatter: function (val) {
                                            return val;
                                        }
                                    }
                                }
                            ]
                        },
                        grid: {
                            borderColor: colors.chartBorderColor,
                        }
                    };

            Chart = new ApexCharts(document.querySelector("#sales-chart"), options);
            Chart.render(); // Re-render the chart


            $('#sales-chart').show();





        }

    }
    salesChart(Tickets, Flights);



    let flight_chart;
    function productsSold() {
        if ($('#products-sold').length) {
            const options = {
                series: [{
                    name: 'Total Tickets',
                    data: flight_days
                }],
                chart: {
                    type: 'bar',
                    height: 380,
                    foreColor: 'rgba(255,255,255,55%)',
                    toolbar: {
                        show: false
                    }
                },
                theme: {
                    mode: $('body').hasClass('dark') ? 'dark' : 'light',
                },
                plotOptions: {
                    bar: {
                        borderRadius: 6,
                        columnWidth: '35%',
                        dataLabels: {
                            position: 'top', // top, center, bottom
                        },
                    }
                },
                colors: ['rgba(255,255,255,60%)'],
                dataLabels: {
                    enabled: true,
                    formatter: function (val) {
                        return val;
                    },
                    offsetY: -20,
                    style: {
                        fontSize: '12px',
                        colors: ['rgba(255,255,255,55%)']
                    }
                },
                xaxis: {
                    show: false,
                    categories: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    },
                },
                yaxis: {
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false,
                    },
                    labels: {
                        show: false,
                        formatter: function (val) {
                            return  val;
                        }
                    }

                },
                grid: {
                    show: false
                }
            };

            flight_chart = new ApexCharts(document.querySelector('#products-sold'), options);
            flight_chart.render();
        }
    }
    productsSold();


    
        $(document).ready(function () {
            // Handle the dropdown change event
            $('#yearDropdown').change(function () {
                var selectedYear = $('#yearDropdown').val();  // Get the selected year

                console.log("Selected Year: " + selectedYear);  // Log the selected year for debugging

                // Make the AJAX request to the backend
                $.ajax({
                    url: '/Admin/Dashboard?handler=Data',  // Update with the correct URL to your backend endpoint
                    method: 'GET',  // Use GET method
                    data: { year: selectedYear },  // Send the selected year as a query parameter
                    success: function (response) {
                        // On success, log and optionally alert the user



                        const updatedTickets = response.ticket;
                        const updatedFlights = response.flight;
                        

                        console.log(updatedTickets);
                        // Example: Update a chart
                        Chart.updateSeries([
                            {
                                name: "Ticket Sold",
                                data: updatedTickets
                            },
                            {
                                name: 'Flight Departures',
                                data: updatedFlights
                            }
                        ]);

                        chart.updateSeries(response.don);

                        flight_chart.updateSeries([
                            {
                                name: 'Total Tickets',
                                data: response.day
                            }
                        ])
                        $('#totalTickets').html(`<i class="bi bi-ticket me-2 text-success"></i> ${response.total_tickets} tickets`);
                        $('#totalFlights').text(`${response.total_flights}`);
                        $('#FinishedStatus').html(` <div class="display-7"> ${response.don[1]} </div> `)
                        $('#DelayedStatus').html(` <div class="display-7"> ${response.don[0]} </div> `)
                        $('#ScheduledStatus').html(` <div class="display-7"> ${response.don[2]} </div> `)

                    },
                    error: function (xhr, status, error) {
                        // On failure, log and alert the user about the error
                        console.error("AJAX Request Failed: " + status + " - " + error);
                        alert('Error updating data.');



                    }


                });


            });

    });
   
    

})
