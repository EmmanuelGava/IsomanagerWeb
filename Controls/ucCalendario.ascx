<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCalendario.ascx.cs" Inherits="IsomanagerWeb.Controls.ucCalendario" %>

<!-- Asegurarnos de que calendar.css se cargue después de bootstrap -->
<link href="<%: ResolveUrl("~/Content/css/calendar.css") %>" rel="stylesheet" type="text/css" />

<div class="week-calendar">
    <div class="calendar-header d-flex justify-content-between align-items-center">
        <h5>Calendario Semanal</h5>
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#fullCalendarModal">
            <i class="bi bi-calendar2-week"></i>
            Ver Calendario Completo
        </button>
    </div>
    <div id="calendar"></div>
</div>

<!-- Modal para calendario completo -->
<div class="modal fade" id="fullCalendarModal" tabindex="-1" role="dialog" aria-labelledby="fullCalendarModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Calendario Completo</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div id="fullCalendar"></div>
                <div class="calendar-sidebar">
                    <div class="upcoming-activities">
                        <h6>Próximas Actividades</h6>
                        <div id="upcomingEventsList">
                            <!-- Los eventos se cargarán dinámicamente aquí -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Modal para detalles del evento -->
<div class="modal fade" id="eventModal" tabindex="-1" aria-labelledby="eventTitle" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="eventTitle"></h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body" id="eventDetails">
            </div>
        </div>
    </div>
</div>

<style>
    /* Estilo para mostrar la inicial */
    .week-calendar .fc-event::before {
        content: attr(data-content);
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        font-size: 14px;
        font-weight: 600;
        color: #333;
    }
</style>

<script type="text/javascript">
    var calendar, fullCalendar;
    
    document.addEventListener('DOMContentLoaded', function() {
        console.log('DOMContentLoaded event fired');
        
        if (window.calendarData) {
            console.log('Datos del calendario disponibles inmediatamente');
            initializeCalendars();
        } else {
            console.warn('Esperando datos del calendario...');
            var checkInterval = setInterval(function() {
                console.log('Verificando datos del calendario...');
                if (window.calendarData) {
                    console.log('Datos del calendario encontrados');
                    clearInterval(checkInterval);
                    initializeCalendars();
                }
            }, 100);
        }

        // Inicializar el modal
        var fullCalendarModal = document.getElementById('fullCalendarModal');
        if (fullCalendarModal) {
            console.log('Modal encontrado');
            fullCalendarModal.addEventListener('show.bs.modal', function () {
                console.log('Modal show event triggered');
            });
            
            fullCalendarModal.addEventListener('shown.bs.modal', function () {
                console.log('Modal shown event triggered');
                if (!fullCalendar) {
                    console.log('Inicializando calendario completo');
                    var fullCalendarEl = document.getElementById('fullCalendar');
                    if (fullCalendarEl) {
                        fullCalendar = new FullCalendar.Calendar(fullCalendarEl, {
                            initialView: 'dayGridMonth',
                            locale: 'es',
                            headerToolbar: {
                                left: 'prev',
                                center: 'title',
                                right: 'next'
                            },
                            events: window.calendarData,
                            height: '100%',
                            dayMaxEvents: false,
                            eventDisplay: 'block',
                            displayEventTime: false,
                            firstDay: 1,
                            dayMaxEventRows: true,
                            eventDidMount: function(info) {
                                // Asignar clases según el tipo y estado
                                if (info.event.extendedProps.tipo === 'auditoria') {
                                    info.el.classList.add('auditoria');
                                    if (info.event.extendedProps.estado) {
                                        // Reemplazar espacios con guiones y convertir a minúsculas
                                        const estadoClase = 'auditoria-' + info.event.extendedProps.estado.toLowerCase().replace(/\s+/g, '-');
                                        info.el.classList.add(estadoClase);
                                    }
                                } else {
                                    info.el.classList.add('proceso');
                                    if (info.event.extendedProps.estado) {
                                        // Reemplazar espacios con guiones y convertir a minúsculas
                                        const estadoClase = 'proceso-' + info.event.extendedProps.estado.toLowerCase().replace(/\s+/g, '-');
                                        info.el.classList.add(estadoClase);
                                    }
                                }
                                
                                // Agregar la inicial como contenido del evento
                                const initial = info.event.extendedProps.tipo === 'auditoria' ? 'A' : 'P';
                                info.el.setAttribute('data-content', initial);
                                
                                new bootstrap.Tooltip(info.el, {
                                    title: info.event.title + ' - ' + 
                                           (info.event.extendedProps.tipo === 'auditoria' ? 'Auditoría' : 'Proceso'),
                                    placement: 'top',
                                    trigger: 'hover'
                                });
                            },
                            eventClick: function(info) {
                                showEventDetails(info.event);
                            }
                        });
                        fullCalendar.render();
                        updateUpcomingEvents();
                        console.log('Calendario completo renderizado');
                    } else {
                        console.error('No se encontró el elemento fullCalendar');
                    }
                } else {
                    console.log('Actualizando calendario existente');
                    fullCalendar.render();
                    updateUpcomingEvents();
                }
            });
        } else {
            console.error('No se encontró el modal del calendario');
        }
    });

    function initializeCalendars() {
        console.log('Iniciando inicialización de calendarios');
        if (!window.calendarData) {
            console.error('No se encontraron datos del calendario');
            return;
        }

        // Calendario principal (vista semanal)
        var calendarEl = document.getElementById('calendar');
        if (calendarEl) {
            console.log('Elemento del calendario semanal encontrado');
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridWeek',
                locale: 'es',
                headerToolbar: false,
                events: window.calendarData,
                height: 'auto',
                dayMaxEvents: false,
                eventDisplay: 'block',
                displayEventTime: false,
                firstDay: 1,
                dayMaxEventRows: true,
                eventDidMount: function(info) {
                    info.el.classList.add(info.event.extendedProps.tipo);
                    if (info.event.extendedProps.estado) {
                        // Reemplazar espacios con guiones y convertir a minúsculas
                        const estadoClase = info.event.extendedProps.tipo + '-' + 
                            info.event.extendedProps.estado.toLowerCase().replace(/\s+/g, '-');
                        info.el.classList.add(estadoClase);
                    }
                    // Agregar la inicial como contenido del evento
                    const initial = info.event.extendedProps.tipo === 'auditoria' ? 'A' : 'P';
                    info.el.setAttribute('data-content', initial);
                    new bootstrap.Tooltip(info.el, {
                        title: info.event.title + ' - ' + 
                               (info.event.extendedProps.tipo === 'auditoria' ? 'Auditoría' : 'Proceso'),
                        placement: 'top',
                        trigger: 'hover'
                    });
                },
                eventClick: function(info) {
                    showEventDetails(info.event);
                }
            });
            calendar.render();
            console.log('Calendario semanal renderizado exitosamente');
        } else {
            console.error('No se encontró el elemento calendar');
        }
    }

    function updateUpcomingEvents() {
        if (!window.calendarData) return;
        
        var today = new Date();
        today.setHours(0, 0, 0, 0); // Establecer la hora a 00:00:00 para comparar solo fechas
        
        var upcomingEvents = window.calendarData
            .filter(function(event) {
                var eventDate = new Date(event.start);
                eventDate.setHours(0, 0, 0, 0); // Establecer la hora a 00:00:00 para comparar solo fechas
                return eventDate >= today; // Incluir todos los eventos futuros, tanto auditorías como procesos
            })
            .sort(function(a, b) {
                return new Date(a.start) - new Date(b.start);
            })
            .slice(0, 5);
        
        var upcomingEventsHtml = upcomingEvents.map(function(event) {
            var eventDate = new Date(event.start);
            var tipo = event.extendedProps.tipo === 'auditoria' ? 'Auditoría' : 'Proceso';
            var className = event.extendedProps.tipo === 'auditoria' ? 'auditoria' : 'proceso';
            if (event.extendedProps.estado) {
                // Reemplazar espacios con guiones y convertir a minúsculas
                className += ' ' + event.extendedProps.tipo + '-' + 
                    event.extendedProps.estado.toLowerCase().replace(/\s+/g, '-');
            }
            return `
                <div class="upcoming-event ${className}" data-bs-toggle="tooltip" title="${event.title} - ${tipo}">
                    <div class="upcoming-event-date">
                        ${eventDate.toLocaleDateString('es-ES', {
                            weekday: 'long',
                            day: 'numeric',
                            month: 'long'
                        })}
                    </div>
                    <div class="upcoming-event-title">${event.title}</div>
                    <div class="upcoming-event-type">
                        <i class="bi ${event.extendedProps.tipo === 'auditoria' ? 'bi-clipboard-check' : 'bi-gear'}"></i>
                        ${tipo}
                    </div>
                </div>
            `;
        }).join('');
        
        document.getElementById('upcomingEventsList').innerHTML = 
            upcomingEvents.length ? upcomingEventsHtml : '<p class="text-muted">No hay actividades próximas</p>';
        
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('.upcoming-event[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(function(tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl, {
                placement: 'left'
            });
        });
    }
</script>