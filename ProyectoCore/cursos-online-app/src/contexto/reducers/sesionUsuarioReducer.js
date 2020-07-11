// este reducer se a encargar de almacenar las variables del usuario
// Los reducer consta de 3 partes
// 1 define los valores que se van a almacenar
// 2 logica
// 3 exportar la funcion
export const initialState = {
  usuario: {
    nombreCompleto: "",
    email: "",
    username: "",
    foto: "",
  },
  autenticado: false,
};

// la data que se va a modificar
// action determina que va hacer con la data de arriba

const sesionUsuarioReducer = (state = initalState, action) => {
  switch (action.type) {
    case "INICIAR_SESION":
      return {
        ...state,
        usuario: action.sesion,
        autenticado: action.autenticado,
      };
    case "SALIR_SESION":
      return {
        ...state,
        usuario: action.nuevoUsuario,
        autenticado: action.autenticado,
      };
    case "ACTUALIZAR_USUARIO":
      return {
        ...state,
        usuario: action.nuevoUsuario,
        autenticado: action.autenticado,
      };
  }
};

export default sesionUsuarioReducer;
