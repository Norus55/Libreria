$(document).ready(function () {
    new DataTable('#example', {
        layout: {
            topStart: {
                buttons: [
                    {
                        extend: 'copy',
                        text: '<i class="fi fi-rr-copy-alt"></i>', // Icono de copiar
                        className: 'botonCopiar btn boton-color-copiar',
                        exportOptions: {
                            columns: ':not(.no-exportar)', // Excluir columnas con la clase 'no-exportar'
                            format: {
                                body: function (data, row, column, node) {
                                    // Aquí puedes personalizar el contenido de la celda si es necesario
                                    return data;
                                }
                            }
                        }
                    },
                    {
                        extend: 'csv',
                        text: '<i class="fi fi-rr-file-csv"></i>', // Icono de CSV
                        className: 'botonCsv btn boton-color-csv',
                        exportOptions: {
                            columns: ':not(.no-exportar)',
                            format: {
                                body: function (data, row, column, node) {
                                    return data;
                                }
                            }
                        }
                    },
                    {
                        extend: 'excel',
                        text: '<i class="fi fi-rr-file-excel"></i>', // Icono de Excel
                        className: 'botonExcel btn boton-color-excel',
                        exportOptions: {
                            columns: ':not(.no-exportar)',
                            format: {
                                body: function (data, row, column, node) {
                                    return data;
                                }
                            }
                        }
                    },
                    {
                        extend: 'pdf',
                        text: '<i class="fi fi-rr-file-pdf"></i>', // Icono de PDF
                        className: 'botonPdf btn boton-color-pdf',
                        exportOptions: {
                            columns: ':not(.no-exportar)',
                            format: {
                                body: function (data, row, column, node) {
                                    return data;
                                }
                            }
                        }
                    },
                    {
                        extend: 'print',
                        text: '<i class="fi fi-rr-print"></i>', // Icono de imprimir
                        className: 'botonPrint btn boton-color-print',
                        exportOptions: {
                            columns: ':not(.no-exportar)',
                            format: {
                                body: function (data, row, column, node) {
                                    return data;
                                }
                            }
                        }
                    },
                    {
                        extend: 'colvis',
                        text: '<i class="fi fi-rr-filter"></i>', // Icono de filtrar
                        className: 'botonColvis btn boton-color-colvis',
                    }
                ]
            }
        },
        language: {
            url: '//cdn.datatables.net/plug-ins/2.0.2/i18n/es-ES.json',
        },
        scrollX: true,
        initComplete: function (json, settings) {
            $(".dt-buttons").removeClass("dt-buttons");
            $(".dt-button").addClass("botones");
        }
    });
});