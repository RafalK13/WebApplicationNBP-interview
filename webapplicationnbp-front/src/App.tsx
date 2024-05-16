import ExchangeRate from "./modules/exchangeRates/ExchangeRate";
import Footer from "./modules/footer/Footer";
import Header from "./modules/header/Header";
import style from "./App.module.scss";

function App() {
  return (
    <div className={style.App}>
      <Header />
      <ExchangeRate />
      <Footer />
    </div>
  );
}

export default App;
