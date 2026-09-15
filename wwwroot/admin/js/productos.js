const nuevoProducto =
    document.getElementById("nuevoProducto");

const formularioProducto =
    document.getElementById("formularioProducto");

const cancelarProducto =
    document.getElementById("cancelarProducto");

const token = sessionStorage.getItem("token");

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
    formularioProducto.hidden = false;
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

    } catch (error) {

        console.error(error);

        mensaje.textContent =
            "No se pudieron cargar los productos.";
    }
}

cargarProductos();

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

        const respuesta = await fetch("/api/Productos", {
            method: "POST",

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
            "Producto creado correctamente.";

        productoForm.reset();

        formularioProducto.hidden = true;

        cargarProductos(); 
   } catch (error) {

        console.error(error);

        mensaje.textContent =
            "No se pudo conectar con el servidor.";
    }
});
