<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="style.css">
    <title>Lista de Tareas</title>
</head>
<body>
    <main class="container">
        <h1>Lista de Tareas</h1>

        <form id="task-form" class="task-form">
            <input id="task-input" type="text" name="tarea" placeholder="Titulo de la tarea..." required>
            <textarea id="task-desc" name="descripcion" placeholder="Descripcion (opcional)..." rows="2"></textarea>
            <button type="submit" name="agregar">Añadir</button>
        </form>

        <ul id="task-list" class="task-list"></ul>

        <div class="filter-bar">
            <button class="filter-btn active" data-filter="all" type="button">Todas</button>
            <button class="filter-btn" data-filter="pending" type="button">Pendientes</button>
            <button class="filter-btn" data-filter="completed" type="button">Completadas</button>
        </div>
    </main>

    <script>
        const apiUrl = 'api.php';
        const taskList = document.getElementById('task-list');
        const taskForm = document.getElementById('task-form');
        const taskInput = document.getElementById('task-input');
        const taskDesc = document.getElementById('task-desc');
        let tareas = [];
        let currentFilter = 'all';

        async function cargarTareas() {
            try {
                const respuesta = await fetch(apiUrl);
                tareas = await respuesta.json();
                renderTareas();
            } catch (error) {
                taskList.innerHTML = '<li class="task-card"><span>No se pudieron cargar las tareas.</span></li>';
            }
        }

        function renderTareas() {
            const tareasFiltradas = tareas.filter((tarea) => {
                if (currentFilter === 'pending') return !tarea.completado;
                if (currentFilter === 'completed') return tarea.completado;
                return true;
            });

            if (tareasFiltradas.length === 0) {
                taskList.innerHTML = '<li class="task-card"><span>No hay tareas en esta vista.</span></li>';
                return;
            }

            taskList.innerHTML = tareasFiltradas.map((tarea) => `
                <li class="task-card ${tarea.completado ? 'completada' : ''}">
                    <input type="checkbox" ${tarea.completado ? 'checked' : ''} data-id="${tarea.id}">
                    <div class="task-info">
                        <span class="task-title">${tarea.titulo}</span>
                        ${tarea.descripcion ? '<span class="task-desc">' + tarea.descripcion + '</span>' : ''}
                        <span class="task-date">${new Date(tarea.fecha).toLocaleDateString('es-ES')}</span>
                    </div>
                    <div class="actions">
                        <button type="button" data-action="delete" data-id="${tarea.id}">🗑</button>
                    </div>
                </li>
            `).join('');
        }

        taskForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            const titulo = taskInput.value.trim();
            if (!titulo) return;

            const descripcion = taskDesc.value.trim();

            await fetch(apiUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    titulo,
                    descripcion,
                    completado: false,
                    fecha: new Date().toISOString()
                })
            });

            taskInput.value = '';
            taskDesc.value = '';
            await cargarTareas();
        });

        taskList.addEventListener('change', async (event) => {
            const checkbox = event.target.closest('input[type="checkbox"]');
            if (!checkbox) return;

            const tareaId = Number(checkbox.dataset.id);
            const tarea = tareas.find((item) => item.id === tareaId);
            if (!tarea) return;

            tarea.completado = checkbox.checked;

            await fetch(`${apiUrl}?id=${tareaId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(tarea)
            });

            await cargarTareas();
        });

        taskList.addEventListener('click', async (event) => {
            const boton = event.target.closest('button[data-action="delete"]');
            if (!boton) return;

            const tareaId = Number(boton.dataset.id);
            await fetch(`${apiUrl}?id=${tareaId}`, { method: 'DELETE' });
            await cargarTareas();
        });

        document.querySelectorAll('.filter-btn').forEach((boton) => {
            boton.addEventListener('click', () => {
                document.querySelectorAll('.filter-btn').forEach((item) => item.classList.remove('active'));
                boton.classList.add('active');
                currentFilter = boton.dataset.filter;
                renderTareas();
            });
        });

        cargarTareas();
    </script>
</body>
</html>
