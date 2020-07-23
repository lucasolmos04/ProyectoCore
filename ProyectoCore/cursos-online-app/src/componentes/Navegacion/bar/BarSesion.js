import React, { useState } from "react";
import {
  Toolbar,
  IconButton,
  Typography,
  makeStyles,
  Button,
  Avatar,
  Drawer,
  List,
  ListItem,
  ListItemText,
} from "@material-ui/core";
import FotoUsuarioTemp from "../../../logo.svg";
import { useStateValue } from "../../../contexto/store";

const useStyles = makeStyles((theme) => ({
  seccionDesktop: {
    display: "none",
    [theme.breakpoints.up("md")]: {
      display: "flex",
    },
  },
  seccionMobile: {
    display: "flex",
    [theme.breakpoints.up("md")]: {
      display: "none",
    },
  },
  grow: {
    flexGrow: 1, // toma todo el espacio disponible
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list: {
    width: 250,
  },
  listItemText: {
    fontSize: "14px",
    fontWeight: 600,
    paddingLeft: "15px",
    color: "#212121",
  },
}));

const BarSesion = () => {
  const classes = useStyles();
  const [{ sesionUsuario }, dispatch] = useStateValue();
  const [abrirMenuIzquierdo, setAbrirMenuIzquierda] = useState(false);
  const cerrarMenuIzquierda = () => {
    setAbrirMenuIzquierda(false);
  };
  const abrirMenuIzquierdaAction = () => {
    setAbrirMenuIzquierda(true);
  };
  return (
    <React.Fragment>
      <Drawer
        open={abrirMenuIzquierdo}
        onClose={cerrarMenuIzquierda}
        anchor="left"
      >
        <div
          className={classes.list}
          onKeyDown={cerrarMenuIzquierda}
          onClick={cerrarMenuIzquierda}
        >
          <List>
            <ListItem button>
              <i className="material-icons">account_box</i>
              <ListItemText
                classes={{ primary: classes.listItemText }}
                primary="Perfil"
              />
            </ListItem>
          </List>
        </div>
      </Drawer>
      <Toolbar>
        <IconButton color="inherit" onClick={abrirMenuIzquierdaAction}>
          <i className="material-icons">menu</i>
        </IconButton>
        <Typography variant="h6">Cursos Online</Typography>
        <div className={classes.grow}></div>
        <div className={classes.seccionDesktop}>
          <Button color="inherit">Salir</Button>
          <Button color="inherit">
            {sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ""}
          </Button>
          <Avatar src={FotoUsuarioTemp}></Avatar>
        </div>
        <div className={classes.seccionMobile}>
          <IconButton color="inherit">
            <i className="material-icons">more_vert</i>
          </IconButton>
        </div>
      </Toolbar>
    </React.Fragment>
  );
};

export default BarSesion;
