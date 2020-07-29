import React, { useState } from "react";
import style from "../Tool/Style";
import {
  Container,
  Avatar,
  Typography,
  TextField,
  Button,
} from "@material-ui/core";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import { loginUsuario } from "../../actions/UsuarioAction";
import { withRouter } from "react-router-dom";
import { useStateValue } from "../../contexto/store";

const Login = (props) => {
  const [{ usuarioSesion }, dispatch] = useStateValue();
  const [usuario, setUsuario] = useState({
    Email: "",
    Password: "",
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;

    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const loginUsuarioBoton = (e) => {
    e.preventDefault(); // no hace un refresh completo de la pagina

    loginUsuario(usuario, dispatch).then((response) => {
      if (response.status === 200) {
        window.localStorage.setItem("token_seguridad", response.data.token);
        props.history.push("/auth/perfil");
      } else {
        dispatch({
          type: "OPEN_SNACKBAR",
          openMensaje: {
            open: true,
            mensaje: "las credenciales del usuario son incorrectas",
          },
        });
      }
    });
  };

  return (
    <Container maxWidth="xs">
      <div style={style.paper}>
        <Avatar style={style.avatar}>
          <LockOutlinedIcon style={style.icon} />
        </Avatar>
        <Typography component="h1" variant="h5">
          Login de Usuario
        </Typography>
        <form style={style.form}>
          <TextField
            variant="outlined"
            name="Email"
            value={usuario.Email}
            onChange={ingresarValoresMemoria}
            label="Ingrese Email"
            fullWidth
            margin="normal"
          />
          <TextField
            variant="outlined"
            name="Password"
            value={usuario.Password}
            onChange={ingresarValoresMemoria}
            type="password"
            label="Ingrese password"
            fullWidth
            margin="normal"
          />
          <Button
            type="submit"
            onClick={loginUsuarioBoton}
            fullWidth
            variant="contained"
            color="primary"
            style={style.submit}
          >
            Enviar
          </Button>
        </form>
      </div>
    </Container>
  );
};

export default withRouter(Login);
