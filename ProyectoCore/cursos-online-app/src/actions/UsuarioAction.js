import HttpCliente from "../servicios/HttpCliente";

export const registrarUsuario = (usuario) => {
  return new Promise((resolve, eject) => {
    HttpCliente.post("/usuario/registrar", usuario).then((response) => {
      resolve(response);
    });
  });
};

export const obtenerUsuarioActual = () => {
  return new Promise((resolve, eject) => {
    HttpCliente.get("/usuario").then((response) => {
      resolve(response);
    });
  });
};

export const actualizarUsuario = (usuario) => {
  return new Promise((resolve, eject) => {
    HttpCliente.put("/usuario", usuario).then((response) => {
      resolve(response);
    });
  });
};

export const loginUsuario = (usuario) => {
  return new Promise((resolve, eject) => {
    HttpCliente.post("/usuario/login", usuario).then((response) => {
      resolve(response);
    });
  });
};
