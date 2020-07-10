import React from "react";
import { ThemeProvider as MuithemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import RegistrarUsuario from "./componentes/seguridad/RegistrarUsuario";
import Login from "./componentes/seguridad/Login";
import Perfil from "./componentes/seguridad/PerfilUsuario";
import PerfilUsuario from "./componentes/seguridad/PerfilUsuario";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { Grid } from "@material-ui/core";
import AppNavbar from "./componentes/Navegacion/AppNavbar";

function App() {
  return (
    <Router>
      <MuithemeProvider theme={theme}>
        <AppNavbar />
        <Grid container>
          <Switch>
            <Route exact path="/auth/login" component={Login} />
            <Route exact path="/auth/registrar" component={RegistrarUsuario} />
            <Route exact path="/auth/perfil" component={PerfilUsuario} />
            <Route exact path="/" component={PerfilUsuario} />
          </Switch>
        </Grid>
      </MuithemeProvider>
    </Router>
  );
}

export default App;
