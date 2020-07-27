export const obtenerDataImagen = (imagen) => {
  return new Promise((resolve, eject) => {
    const nombre = imagen.name;
    const extension = imagen.name.split(".").pop();

    const lector = new FileReader(); // Lee la data que esta ingresando
    lector.readAsDataURL(imagen);
    lector.onload = () =>
      resolve({
        data: lector.result.split(",")[1],
        nombre: nombre,
        extension: extension,
      }); // crea el base 64

    lector.onerror = (error) => PromiseRejectionEvent(error);
  });
};
