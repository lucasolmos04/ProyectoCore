import React, { createContext, useContext, useReducer } from "react";

export const StateContext = createContext();

// se define el provider
// La unica forma de cambiar el valor global es con el reducer
// initalState es el storage donde se guardan las variables globales
// children son todos los componentes de reack hooks
export const StateProvider = ({ reducer, initialState, children }) => (
  // El provider es el que sibscribe todos los objetos que van a tener acceso a las variables globales
  <StateContext.Provider value={useReducer(reducer, initialState)}>
    {children}
  </StateContext.Provider> // se subscribe todos los children
);

// se define un consumer
// useContext te da acceso a todas las variables globales almacenadas en el contexto
export const useStateValue = () => useContext(StateContext);
