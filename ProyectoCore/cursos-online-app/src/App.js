import React, { useState, useEffect } from "react";
import { ThemeProvider as MuithemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import RegistrarUsuario from "./componentes/seguridad/RegistrarUsuario";
import Login from "./componentes/seguridad/Login";
import Perfil from "./componentes/seguridad/PerfilUsuario";
import PerfilUsuario from "./componentes/seguridad/PerfilUsuario";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { Grid, Snackbar } from "@material-ui/core";
import AppNavbar from "./componentes/Navegacion/AppNavbar";
import { useStateValue } from "./contexto/store";
import { obtenerUsuarioActual } from "./actions/UsuarioAction";
import RutaSegura from "./componentes/Navegacion/RutaSegura";
import NuevoCurso from "./componentes/cursos/NuevoCurso";
import PaginadorCurso from "./componentes/cursos/PaginadorCurso";

function App() {
  // obtengo la referencia de la variable global de usuario
  // dispatch es una representacion del contexto
  const [{ sesionUsuario, openSnackbar }, dispatch] = useStateValue();

  const [iniciaApp, setIniciaApp] = useState(false);
  useEffect(() => {
    if (!iniciaApp) {
      obtenerUsuarioActual(dispatch)
        .then((response) => {
          setIniciaApp(true);
        })
        .catch((error) => {
          setIniciaApp(true);
        });
    }
  }, [iniciaApp]);
  // iniciaApp bandera para detectar que sesion de usuario se inicio correctamente
  return iniciaApp === false ? null : (
    <React.Fragment>
      <Snackbar
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
        open={openSnackbar ? openSnackbar.open : false}
        autoHideDuration={3000}
        ContentProps={{ "area-describedby": "message-id" }}
        message={
          <span id="message-id">
            {openSnackbar ? openSnackbar.mensaje : ""}
          </span>
        }
        onClose={() =>
          dispatch({
            type: "OPEN_SNACKBAR",
            openMensaje: {
              open: false,
              mensaje: "",
            },
          })
        }
      ></Snackbar>

      <Router>
        <MuithemeProvider theme={theme}>
          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route
                exact
                path="/auth/registrar"
                component={RegistrarUsuario}
              />

              <RutaSegura exact path="/auth/perfil" component={PerfilUsuario} />
              <RutaSegura exact path="/" component={PerfilUsuario} />
              <RutaSegura exact path="/curso/nuevo" component={NuevoCurso} />

              <RutaSegura
                exact
                path="/curso/paginador"
                component={PaginadorCurso}
              />
            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
