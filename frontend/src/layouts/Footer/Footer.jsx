import { Link } from 'react-router-dom';
import { FiFacebook, FiInstagram, FiTwitter, FiYoutube, FiMail, FiPhone, FiMapPin } from 'react-icons/fi';
import './Footer.css';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        <div className="footer-grid">
          {/* Brand */}
          <div className="footer-brand">
            <Link to="/" className="footer-logo">
              <span className="logo-text">LUXE</span>
              <span className="logo-accent">Store</span>
            </Link>
            <p className="footer-description">
              Descubra produtos exclusivos e de alta qualidade. Elegância e sofisticação em cada detalhe.
            </p>
            <div className="social-links">
              <a href="#" className="social-link" aria-label="Facebook">
                <FiFacebook />
              </a>
              <a href="#" className="social-link" aria-label="Instagram">
                <FiInstagram />
              </a>
              <a href="#" className="social-link" aria-label="Twitter">
                <FiTwitter />
              </a>
              <a href="#" className="social-link" aria-label="YouTube">
                <FiYoutube />
              </a>
            </div>
          </div>

          {/* Quick Links */}
          <div className="footer-links">
            <h4 className="footer-title">Links Rápidos</h4>
            <ul>
              <li><Link to="/">Home</Link></li>
              <li><Link to="/products">Produtos</Link></li>
              <li><Link to="/categories">Categorias</Link></li>
              <li><Link to="/about">Sobre Nós</Link></li>
              <li><Link to="/contact">Contato</Link></li>
            </ul>
          </div>

          {/* Customer Service */}
          <div className="footer-links">
            <h4 className="footer-title">Atendimento</h4>
            <ul>
              <li><Link to="/faq">FAQ</Link></li>
              <li><Link to="/shipping">Envio</Link></li>
              <li><Link to="/returns">Trocas e Devoluções</Link></li>
              <li><Link to="/payment">Formas de Pagamento</Link></li>
              <li><Link to="/tracking">Rastrear Pedido</Link></li>
            </ul>
          </div>

          {/* Contact */}
          <div className="footer-contact">
            <h4 className="footer-title">Contato</h4>
            <ul>
              <li>
                <FiMapPin />
                <span>Av. Principal, 1000<br />São Paulo - SP</span>
              </li>
              <li>
                <FiPhone />
                <span>(11) 99999-9999</span>
              </li>
              <li>
                <FiMail />
                <span>contato@luxestore.com</span>
              </li>
            </ul>
          </div>
        </div>

        {/* Bottom */}
        <div className="footer-bottom">
          <p>&copy; {new Date().getFullYear()} LUXE Store. Todos os direitos reservados.</p>
          <div className="footer-legal">
            <Link to="/privacy">Privacidade</Link>
            <Link to="/terms">Termos de Uso</Link>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
