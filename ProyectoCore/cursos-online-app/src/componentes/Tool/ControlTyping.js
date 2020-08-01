import React, { useState, useEffect } from "react";

export default function ControlTyping(texto, delay) {
  const [textoValor, setTextoValor] = useState();

  // esta funcion me va a permitir manejar el tiempo de la caja de texto en que esta escribiendo el usuario
  useEffect(() => {
    const manejador = setTimeout(() => {
      setTextoValor(texto);
    }, delay); // Si deja de escribir toma el valor que hay en la caja de texto

    return () => {
      clearTimeout(manejador);
    };
  }, [texto]);

  return textoValor;
}
