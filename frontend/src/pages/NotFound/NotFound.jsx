import { Link } from 'react-router-dom';
import './NotFound.css';

const NotFound = () => {
  return (
    <main className="not-found-page">
      <div className="container">
        <div className="not-found-content">
          <h1>404</h1>
          <h2>Página não encontrada</h2>
          <p>A página que você está procurando não existe ou foi movida.</p>
          <Link to="/" className="btn btn-primary">
            Voltar ao Início
          </Link>
        </div>
      </div>
    </main>
  );
};

export default NotFound;
