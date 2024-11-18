// crud-operations.js
$(document).ready(function () {
    // Inicializar DataTable para todas las tablas con id 'example'
    if ($.fn.DataTable) {
        $('#example').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
            }
        });
    }

    // Función genérica para cargar detalles
    function loadEntityDetails(url, entityType) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    const data = response.data;
                    let detailsHtml = '<div class="text-left">';

                    // Generar HTML basado en el tipo de entidad
                    switch (entityType) {
                        case 'Categorias':
                            detailsHtml += `
                                <p><strong>Código:</strong> ${data.codigoCategoria}</p>
                                <p><strong>Nombre:</strong> ${data.nombre}</p>
                            `;
                            break;
                        case 'Editoriales':
                            detailsHtml += `
                                <p><strong>NIT:</strong> ${data.nit}</p>
                                <p><strong>Nombre:</strong> ${data.nombres}</p>
                                <p><strong>Teléfono:</strong> ${data.telefono}</p>
                                <p><strong>Dirección:</strong> ${data.direccion}</p>
                                <p><strong>Email:</strong> ${data.email}</p>
                                <p><strong>Sitio Web:</strong> ${data.sitioweb}</p>
                            `;
                            break;
                        case 'Autores':
                            detailsHtml += `
                                <p><strong>ID:</strong> ${data.idAutor}</p>
                                <p><strong>Nombre:</strong> ${data.nombre}</p>
                                <p><strong>Apellido:</strong> ${data.apellido}</p>
                                <p><strong>Nacionalidad:</strong> ${data.nacionalidad}</p>
                            `;
                            break;
                        case 'LibrosAutores':
                            detailsHtml += `
                                <p><strong>ID:</strong> ${data.idLibrosAutor}</p>
                                <p><strong>Autor:</strong> ${data.nombreAutor}</p>
                                <p><strong>Libro:</strong> ${data.tituloLibro}</p>
                            `;
                            break;
                    }
                    detailsHtml += '</div>';

                    Swal.fire({
                        title: `Detalles de ${entityType.slice(0, -1)}`,
                        html: detailsHtml,
                        width: '600px'
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.message || 'Error al cargar los detalles',
                        icon: 'error'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudieron cargar los detalles',
                    icon: 'error'
                });
            }
        });
    }

    // Función genérica para eliminar
    window.confirmDelete = function (id, name, entityType) {
        Swal.fire({
            title: '¿Estás seguro?',
            text: `¿Quieres eliminar ${entityType.slice(0, -1)} "${name}"?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/${entityType}/DeleteAjax`,
                    type: 'POST',
                    data: { id: id },
                    success: function (result) {
                        if (result.success) {
                            Swal.fire({
                                title: '¡Eliminado!',
                                text: result.message,
                                icon: 'success'
                            }).then(() => {
                                location.reload();
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: result.message || 'Error al eliminar',
                                icon: 'error'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            title: 'Error',
                            text: 'Ocurrió un error al intentar eliminar.',
                            icon: 'error'
                        });
                    }
                });
            }
        });
    };

    // Manejo de formularios
    function setupFormHandler(form, entityType) {
        form.on('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(this);
            const isEdit = form.attr('id') === 'editForm';
            const url = isEdit ?
                `/${entityType}/Edit/${formData.get('IdAutor') || formData.get('id')}` :
                `/${entityType}/Create`;

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: '¡Éxito!',
                            text: response.message || 'Operación completada con éxito',
                            icon: 'success'
                        }).then(() => {
                            window.location.href = `/${entityType}`;
                        });
                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: response.message || 'Hubo un error al procesar la solicitud',
                            icon: 'error'
                        });
                    }
                },
                error: function (xhr) {
                    let errorMessage = 'Hubo un error al procesar la solicitud';
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        errorMessage = xhr.responseJSON.message;
                    }
                    Swal.fire({
                        title: 'Error',
                        text: errorMessage,
                        icon: 'error'
                    });
                }
            });
        });
    }

    // Inicializar manejadores de formularios
    if ($('#createForm').length) {
        const entityType = window.location.pathname.split('/')[1];
        setupFormHandler($('#createForm'), entityType);
    }
    if ($('#editForm').length) {
        const entityType = window.location.pathname.split('/')[1];
        setupFormHandler($('#editForm'), entityType);
    }

    // Evento para los botones de detalles
    $(document).on('click', '.btn-details', function () {
        const id = $(this).data('id');
        const entityType = $(this).data('entity');
        loadEntityDetails(`/${entityType}/Details/${id}`, entityType);
    });
});