import HttpCliente from "../servicios/HttpCliente";
import axios from "axios";

// Se crea instancia solo para login y para RegistrarUsuaario ya que no necesitan el Token de seguridad
const instancia = axios.create();
instancia.CancelToken = axios.CancelToken;
instancia.isCancel = axios.isCancel;

export const registrarUsuario = (usuario) => {
  return new Promise((resolve, eject) => {
    instancia.post("/usuario/registrar", usuario).then((response) => {
      resolve(response);
    });
  });
};

export const obtenerUsuarioActual = (dispatch) => {
  return new Promise((resolve, eject) => {
    HttpCliente.get("/usuario")
      .then((response) => {
        if (response.data && response.data.imagenPerfil) {
          let fotoPerfil = response.data.imagenPerfil;
          const nuevoFile =
            "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
          response.data.imagenPerfil = nuevoFile;
        }

        dispatch({
          type: "INICIAR_SESION",
          sesion: response.data,
          autenticado: true,
        });
        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};

export const actualizarUsuario = (usuario, dispatch) => {
  return new Promise((resolve, eject) => {
    HttpCliente.put("/usuario", usuario)
      .then((response) => {
        if (response.data && response.data.imagenPerfil) {
          let fotoPerfil = response.data.imagenPerfil;
          const nuevoFile =
            "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
          response.data.imagenPerfil = nuevoFile;
        }

        dispatch({
          type: "INICIAR_SESION", // Refresca toda la data del usuario que esta en sesion
          sesion: response.data,
          autenticado: true,
        });

        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};

export const loginUsuario = (usuario, dispatch) => {
  return new Promise((resolve, eject) => {
    instancia
      .post("/usuario/login", usuario)
      .then((response) => {
        if (response.data && response.data.imagenPerfil) {
          let fotoPerfil = response.data.imagenPerfil;
          const nuevoFile =
            "data:imagen/" +
            fotoPerfil.extension +
            ";base64," +
            fotoPerfil.data;
          response.data.imagenPerfil = nuevoFile;
        }

        dispatch({
          type: "INICIAR_SESION",
          sesion: response.data,
          autenticado: true,
        });

        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};
