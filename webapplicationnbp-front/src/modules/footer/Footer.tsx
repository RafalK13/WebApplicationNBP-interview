import React from "react";
import style from "./Footer.module.scss";

const footerDescription = "Created by Rafał Karczewski";

const Footer = () => {
  return (
    <footer className={style.container}>
      <h3>{footerDescription}</h3>
    </footer>
  );
};

export default Footer;
