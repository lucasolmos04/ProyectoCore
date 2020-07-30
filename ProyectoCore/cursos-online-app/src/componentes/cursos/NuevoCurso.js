import React, { useState } from "react";
import {
  Container,
  Typography,
  Grid,
  TextField,
  Button,
} from "@material-ui/core";
import style from "../Tool/Style";
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
} from "@material-ui/pickers"; // Layout de fechas
import DateFnsUtils from "@date-io/date-fns"; // Da soporte a las fechas
import ImageUploader from "react-images-upload";
import { v4 as uuidv4 } from "uuid";
import { obtenerDataImagen } from "../../actions/ImagenAction";
import { guardarCurso } from "../../actions/CursoAction";
import { useStateValue } from "../../contexto/store";

const NuevoCurso = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();
  const [fechaSeleccionada, setFechaSeleccionada] = useState(new Date());
  const [imagenCurso, setImagenCurso] = useState(null);
  const [curso, setCurso] = useState({
    titulo: "",
    descripcion: "",
    precio: 0.0,
    promocion: 0.0,
  });

  const resetearForm = () => {
    setFechaSeleccionada(new Date());
    setImagenCurso(null);
    setCurso({
      titulo: "",
      descripcion: "",
      precio: 0.0,
      promocion: 0.0,
    });
  };

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target; // Obtiene los valores de la caja de texo (name y value)
    setCurso((anterior) => ({
      // Funcion para setear Curso
      ...anterior, // Mantiene el valor anterior de curso
      [name]: value, // Setea el valor de la caja de texto
    }));
  };

  const subirFoto = (imagenes) => {
    const foto = imagenes[0];
    obtenerDataImagen(foto).then((respuesta) => {
      setImagenCurso(respuesta);
    });
  };

  const guardarCursoBoton = (e) => {
    e.preventDefault();

    const cursoId = uuidv4();

    const objetoCurso = {
      titulo: curso.titulo,
      descripcion: curso.descripcion,
      promocion: parseFloat(curso.promocion || 0.0),
      precio: parseFloat(curso.precio || 0.0),
      fechaPublicacion: fechaSeleccionada,
      cursoId: cursoId,
    };

    let objetoImagen = null;
    if (imagenCurso) {
      objetoImagen = {
        nombre: imagenCurso.nombre,
        data: imagenCurso.data,
        extension: imagenCurso.extension,
        objetoReferencia: cursoId,
      };
    }

    guardarCurso(objetoCurso, objetoImagen).then((respuestas) => {
      const responseCurso = respuestas[0];
      const responseImagen = respuestas[1];
      let mensaje = "";

      if (responseCurso.status === 200) {
        mensaje += "Se guardo exitosamente el curso";
        resetearForm();
      } else {
        mensaje += "Errores: " + Object.keys(responseCurso.data.errors);
      }

      if (responseImagen) {
        if (responseImagen.status === 200) {
          mensaje += ", se guardo la imagen correctamente";
        } else {
          mensaje +=
            "Errores en Imagen: " + Object.keys(responseImagen.data.errors);
        }
      }

      dispatch({
        type: "OPEN_SNACKBAR",
        openMensaje: {
          open: true,
          mensaje: mensaje,
        },
      });
    });
  };

  const fotoKey = uuidv4();

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registro de Nuevo Curso
        </Typography>
        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField
                name="titulo"
                variant="outlined"
                fullWidth
                label="Ingrese Titulo"
                value={curso.titulo}
                onChange={ingresarValoresMemoria}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="descripcion"
                variant="outlined"
                fullWidth
                label="Ingrese Descripcion"
                value={curso.descripcion}
                onChange={ingresarValoresMemoria}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="precio"
                variant="outlined"
                fullWidth
                label="Ingrese Precio Normal"
                value={curso.precio}
                onChange={ingresarValoresMemoria}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="promocion"
                variant="outlined"
                fullWidth
                label="Ingrese Precio Promocion"
                value={curso.promocion}
                onChange={ingresarValoresMemoria}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <KeyboardDatePicker
                  value={fechaSeleccionada}
                  onChange={setFechaSeleccionada}
                  margin="normal"
                  id="fecha-publicacion-id"
                  label="Seleccione Fecha de Publicacion"
                  format="dd/MM/yyyy"
                  fullWidth
                  KeyboardButtonProps={{
                    "aria-label": "change date",
                  }}
                />
              </MuiPickersUtilsProvider>
            </Grid>
            <Grid item xs={12} md={6}>
              <ImageUploader
                withIcon={false}
                key={fotoKey}
                singleImage={true}
                buttonText="Seleccion image del curso"
                onChange={subirFoto}
                imgExtension={[".jpg", ".gif", "png", ".jpeg"]}
                maxFileSize={5242880}
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
                style={style.submit}
                onClick={guardarCursoBoton}
              >
                Guardar Curso
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default NuevoCurso;
