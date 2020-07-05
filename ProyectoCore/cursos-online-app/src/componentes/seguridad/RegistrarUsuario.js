import React from "react";
import {
  Container,
  Typography,
  Grid,
  TextField,
  Button,
} from "@material-ui/core";

const style = {
  paper: {
    marginTop: 8,
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  form: {
    width: "100%",
    margin: 20,
  },
  submit: {
    marginTop: 15,
  },
};

const RegistrarUsuario = () => {
  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registro de Usuario
        </Typography>
        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField
                name="nombre"
                variant="outlined"
                fullWidth
                label="Ingrese su nombre"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="apellidos"
                variant="outlined"
                fullWidth
                label="Ingrese sus apellidos"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="email"
                variant="outlined"
                fullWidth
                label="Ingrese su email"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="username"
                variant="outlined"
                fullWidth
                label="Ingrese su username"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="password"
                variant="outlined"
                fullWidth
                label="Ingrese password"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="confirmacionPassword"
                variant="outlined"
                fullWidth
                label="Confirme su password"
              />
            </Grid>
          </Grid>
          <Grid container justify="center">
            <Grid item xs={12} md={6}>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                color="primary"
                size="large"
                style={style.submit}
              >
                Enviar
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default RegistrarUsuario;
