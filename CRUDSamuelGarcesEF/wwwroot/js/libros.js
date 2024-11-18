$(document).ready(function () {
    // Inicializar DataTable si existe
    if ($.fn.DataTable && $('#example').length) {
        $('#example').DataTable();
    }

    // Función para mostrar detalles del libro
    function loadBookDetails(isbn) {
        $.ajax({
            url: `/Libros/Details/${isbn}`,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    const libro = response.data;
                    Swal.fire({
                        title: 'Detalles del Libro',
                        html: `
                            <div class="text-left">
                                <p><strong>ISBN:</strong> ${libro.isbn}</p>
                                <p><strong>Título:</strong> ${libro.titulo}</p>
                                <p><strong>Descripción:</strong> ${libro.descripcion}</p>
                                <p><strong>Fecha de Publicación:</strong> ${libro.publicacion}</p>
                                <p><strong>Fecha de Registro:</strong> ${libro.fechaRegistro}</p>
                                <p><strong>Categoría:</strong> ${libro.categoria}</p>
                                <p><strong>Editorial:</strong> ${libro.editorial}</p>
                            </div>
                        `,
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
                    text: 'No se pudieron cargar los detalles del libro',
                    icon: 'error'
                });
            }
        });
    }

    // Función para eliminar libro
    window.confirmDelete = function (isbn, title) {
        Swal.fire({
            title: '¿Estás seguro?',
            text: `¿Quieres eliminar el libro '${title}'?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Libros/DeleteAjax',
                    type: 'POST',
                    data: { id: isbn },
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
                            text: 'Ocurrió un error al intentar eliminar el libro.',
                            icon: 'error'
                        });
                    }
                });
            }
        });
    };

    // Evento para el botón de detalles
    $(document).on('click', '.btn-details', function () {
        const isbn = $(this).data('isbn');
        loadBookDetails(isbn);
    });

    // Manejo del formulario de creación
    if ($('#createForm').length) {
        $('#createForm').on('submit', function (e) {
            e.preventDefault();
            let formData = new FormData(this);

            $.ajax({
                url: '/Libros/Create',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: '¡Éxito!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            window.location.href = '/Libros';
                        });
                    } else {
                        let errorMessage = response.message;
                        if (response.errors && response.errors.length > 0) {
                            errorMessage += '\n' + response.errors.join('\n');
                        }
                        Swal.fire({
                            title: 'Error',
                            text: errorMessage,
                            icon: 'error'
                        });
                    }
                },
                error: function (xhr) {
                    Swal.fire({
                        title: 'Error',
                        text: 'Hubo un error al procesar la solicitud',
                        icon: 'error'
                    });
                }
            });
        });
    }

    // Manejo del formulario de edición
    if ($('#editForm').length) {
        $('#editForm').on('submit', function (e) {
            e.preventDefault();
            let formData = new FormData(this);
            let isbn = formData.get('Isbn');

            $.ajax({
                url: `/Libros/Edit/${isbn}`,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: '¡Éxito!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            window.location.href = '/Libros';
                        });
                    } else {
                        let errorMessage = response.message;
                        if (response.errors && response.errors.length > 0) {
                            errorMessage += '\n' + response.errors.join('\n');
                        }
                        Swal.fire({
                            title: 'Error',
                            text: 'Error en la edición', errorMessage,
                            icon: 'error'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'Hubo un error al procesar la solicitud',
                        icon: 'error'
                    });
                }
            });
        });
    }
});