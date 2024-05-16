import { useEffect, useState } from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import style from "./ExchangeRate.module.scss";

interface IExchangeRate {
  effectiveDate: string;
  rates: IRate[];
}

interface IRate {
  currency: string;
  code: string;
  mid: number;
}

const dateDescription = "Data current as of ";

const ExchangeRate = () => {
  const [data, setData] = useState<IExchangeRate | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch("http://localhost:5238/api/ExchangeRates");
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        const jsonData = await response.json();

        console.log(jsonData);

        setData(jsonData);
      } catch (err: any) {
        throw Error(err.message || "Something went wrong!");
      }
    };

    fetchData();
  }, []);

  const getDate = (date: string | undefined) =>
    date ? new Date(date).toLocaleDateString("pl-PL") : "";

  return (
    <section className={style.container}>
      <div className={style.dateDescription}>
        <p>{`${dateDescription} ${getDate(data?.effectiveDate)}`}</p>
      </div>

      <TableContainer component={Paper}>
        <Table stickyHeader aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell align="left"> Code</TableCell>
              <TableCell align="right">Name</TableCell>
              <TableCell align="right">Rate</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {data?.rates &&
              data?.rates.map((row, index) => (
                <TableRow key={index}>
                  <TableCell>{row.code}</TableCell>
                  <TableCell align="right">{row.currency}</TableCell>
                  <TableCell align="right">{row.mid}</TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>
      </TableContainer>
    </section>
  );
};

export default ExchangeRate;
