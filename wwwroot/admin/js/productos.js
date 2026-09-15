const nuevoProducto =
    document.getElementById("nuevoProducto");

const formularioProducto =
    document.getElementById("formularioProducto");

const cancelarProducto =
    document.getElementById("cancelarProducto");

const token = sessionStorage.getItem("token");

let productoEditandoId = null;

const listaProductos =
    document.getElementById("listaProductos");

const mensaje =
    document.getElementById("mensaje");

const cerrarSesion =
    document.getElementById("cerrarSesion");

if (!token) {
    window.location.href = "/admin/login.html";
}

cerrarSesion.addEventListener("click", () => {
    sessionStorage.removeItem("token");

    window.location.href = "/admin/login.html";
});

nuevoProducto.addEventListener("click", () => {

    productoEditandoId = null;

    productoForm.reset();

    document.getElementById("tituloFormulario").textContent =
        "Nuevo producto";

    formularioProducto.hidden = false;

    mensaje.textContent = "";
});

cancelarProducto.addEventListener("click", () => {
    productoForm.reset();

    formularioProducto.hidden = true;

    mensaje.textContent = "";
});

async function cargarProductos() {

    mensaje.textContent = "Cargando productos...";

    try {

        const respuesta = await fetch("/api/Productos");

        if (!respuesta.ok) {
            throw new Error("No se pudieron obtener los productos.");
        }

        const productos = await respuesta.json();

        listaProductos.innerHTML = "";

        if (productos.length === 0) {
            mensaje.textContent = "No hay productos registrados.";
            return;
        }

        mensaje.textContent = "";

        productos.forEach(producto => {

            const tarjeta = document.createElement("article");

            tarjeta.classList.add("producto-card");

            tarjeta.innerHTML = `
                <h3>${producto.nombre}</h3>

                <p class="producto-precio">
                    $${producto.precio}
                </p>

                <p>
                    ${producto.descripcion ?? ""}
                </p>

                <p>
                    Categoría:
                    ${producto.categoria ?? "Sin categoría"}
                </p>

                <p>
                    ${producto.disponible ? "Disponible" : "No disponible"}
                </p>

                <div class="producto-acciones">
    <button class="btn-editar" data-id="${producto.id}">
        Editar
    </button>

    <button class="btn-eliminar" data-id="${producto.id}">
        Eliminar
    </button>
</div>
            `;

            listaProductos.appendChild(tarjeta);
        });

document.querySelectorAll(".btn-editar").forEach(boton => {

    boton.addEventListener("click", () => {

        const id = Number(boton.dataset.id);

        const producto = productos.find(p => p.id === id);

        if (!producto) {
            mensaje.textContent = "No se encontró el producto.";
            return;
        }

        editarProducto(producto);
    });

});
        document.querySelectorAll(".btn-eliminar").forEach(boton => {

    boton.addEventListener("click", async () => {

        const id = Number(boton.dataset.id);

        const confirmar = confirm(
            "¿Seguro que quieres eliminar este producto?"
        );

        if (!confirmar) {
            return;
        }

        mensaje.textContent = "Eliminando producto...";

        try {

            const respuesta = await fetch(`/api/Productos/${id}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            });

            if (!respuesta.ok) {
                const datos = await respuesta.text();

                mensaje.textContent =
                    datos || "No se pudo eliminar el producto.";

                return;
            }

            mensaje.textContent =
                "Producto eliminado correctamente.";

            await cargarProductos();

        } catch (error) {

            console.error(error);

            mensaje.textContent =
                "No se pudo conectar con el servidor.";
        }
    });

});

    } catch (error) {

        console.error(error);

        mensaje.textContent =
            "No se pudieron cargar los productos.";
    }
}

cargarProductos();

function editarProducto(producto) {
productoEditandoId = producto.id;
    formularioProducto.hidden = false;

    document.getElementById("tituloFormulario").textContent =
        "Editar producto";

    document.getElementById("nombre").value =
        producto.nombre;

    document.getElementById("precio").value =
        producto.precio;

    document.getElementById("descripcion").value =
        producto.descripcion ?? "";

    document.getElementById("categoria").value =
        producto.categoria ?? "";

    document.getElementById("imagenUrl").value =
        producto.imagenUrl ?? "";

    document.getElementById("disponible").checked =
        producto.disponible;
}
const productoForm =
    document.getElementById("productoForm");

productoForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const producto = {
        nombre: document.getElementById("nombre").value.trim(),
        precio: Number(document.getElementById("precio").value),
        descripcion: document.getElementById("descripcion").value.trim(),
        categoria: document.getElementById("categoria").value.trim(),
        imagenUrl: document.getElementById("imagenUrl").value.trim(),
        disponible: document.getElementById("disponible").checked
    };

    mensaje.textContent = "Guardando producto...";

    try {

        const url = productoEditandoId === null
    ? "/api/Productos"
    : `/api/Productos/${productoEditandoId}`;

const metodo = productoEditandoId === null
    ? "POST"
    : "PUT";

const respuesta = await fetch(url, {
    method: metodo,

            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },

            body: JSON.stringify(producto)
        });

        let datos;

        try {
            datos = await respuesta.json();
        } catch {
            datos = await respuesta.text();
        }

        if (!respuesta.ok) {

            mensaje.textContent =
                datos.mensaje ||
                datos ||
                "No se pudo crear el producto.";

            return;
        }

        mensaje.textContent =
            "Producto guardando correctamente.";

        productoForm.reset();

        formularioProducto.hidden = true;
      productoEditandoId = null;

        cargarProductos();
      
   } catch (error) {

        console.error(error);

        mensaje.textContent =
            "No se pudo conectar con el servidor.";
    }
});
