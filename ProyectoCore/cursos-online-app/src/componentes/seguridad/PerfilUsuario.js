import React, { useState, useEffect } from "react";
import style from "../Tool/Style";
import {
  Container,
  Typography,
  TextField,
  Button,
  Grid,
} from "@material-ui/core";
import {
  obtenerUsuarioActual,
  actualizarUsuario,
} from "../../actions/UsuarioAction";

const PerfilUsuario = () => {
  const [usuario, setUsuario] = useState({
    nombreCompleto: "",
    email: "",
    password: "",
    confirmarPassword: "",
    username: "",
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;
    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  useEffect(() => {
    obtenerUsuarioActual().then((response) => {
      console.log("Data del objeto response. Usuario actual", response);
      setUsuario(response.data);
    });
  }, []);

  const guardarUsuario = (e) => {
    e.preventDefault();
    actualizarUsuario(usuario).then((response) => {
      console.log("se actualizo el usuario", usuario);
      window.localStorage.setItem("token_seguridad", response.data.toke);
    });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography componet="h1" variant="h5">
          Perfil de Usuario
        </Typography>
      </div>
      <form style={style.form}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={12}>
            <TextField
              name="nombreCompleto"
              value={usuario.nombreCompleto}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Ingrese nombre y apellidos"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="username"
              value={usuario.username}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Ingrese Username"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="email"
              value={usuario.email}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Ingrese email"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="password"
              onChange={ingresarValoresMemoria}
              type="password"
              variant="outlined"
              fullWidth
              label="Ingrese password"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="confirmarPassword"
              onChange={ingresarValoresMemoria}
              type="password"
              variant="outlined"
              fullWidth
              label="Confirme password"
            />
          </Grid>
        </Grid>
        <Grid container justify="center">
          <Grid item xs={12} md={6}>
            <Button
              type="submit"
              onClick={guardarUsuario}
              fullWidth
              variant="contained"
              size="large"
              color="primary"
              style={style.submit}
            >
              Guardar Datos
            </Button>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default PerfilUsuario;
