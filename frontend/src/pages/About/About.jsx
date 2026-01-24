import { FiAward, FiHeart, FiTruck, FiShield } from 'react-icons/fi';
import './About.css';

const About = () => {
  const values = [
    { icon: <FiAward />, title: 'Qualidade Premium', description: 'Selecionamos apenas os melhores produtos para nossos clientes.' },
    { icon: <FiHeart />, title: 'Paixão pelo Cliente', description: 'Cada cliente é único e merece uma experiência excepcional.' },
    { icon: <FiTruck />, title: 'Entrega Rápida', description: 'Logística eficiente para entregar no menor tempo possível.' },
    { icon: <FiShield />, title: 'Compra Segura', description: 'Seus dados protegidos com a mais alta tecnologia.' },
  ];

  return (
    <main className="about-page">
      {/* Hero */}
      <section className="about-hero">
        <div className="container">
          <h1>Sobre a LUXE Store</h1>
          <p>Elegância e sofisticação em cada detalhe</p>
        </div>
      </section>

      {/* Story */}
      <section className="about-story">
        <div className="container">
          <div className="story-grid">
            <div className="story-image">
              <img src="https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=600" alt="Nossa loja" />
            </div>
            <div className="story-content">
              <h2>Nossa História</h2>
              <p>
                Fundada em 2020, a LUXE Store nasceu com a missão de trazer produtos premium 
                e exclusivos para nossos clientes. Acreditamos que todos merecem ter acesso 
                a itens de alta qualidade que combinam design sofisticado e funcionalidade.
              </p>
              <p>
                Nossa equipe trabalha incansavelmente para selecionar os melhores produtos 
                de marcas renomadas, garantindo uma experiência de compra única e satisfatória.
              </p>
              <p>
                Mais do que uma loja, somos um destino para quem busca o extraordinário. 
                Cada produto em nosso catálogo foi cuidadosamente escolhido para superar 
                suas expectativas.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Values */}
      <section className="about-values">
        <div className="container">
          <h2>Nossos Valores</h2>
          <div className="values-grid">
            {values.map((value, index) => (
              <div key={index} className="value-card">
                <div className="value-icon">{value.icon}</div>
                <h3>{value.title}</h3>
                <p>{value.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Numbers */}
      <section className="about-numbers">
        <div className="container">
          <div className="numbers-grid">
            <div className="number-item">
              <span className="number">10K+</span>
              <span className="label">Clientes Satisfeitos</span>
            </div>
            <div className="number-item">
              <span className="number">500+</span>
              <span className="label">Produtos Premium</span>
            </div>
            <div className="number-item">
              <span className="number">50+</span>
              <span className="label">Marcas Parceiras</span>
            </div>
            <div className="number-item">
              <span className="number">5</span>
              <span className="label">Anos de Mercado</span>
            </div>
          </div>
        </div>
      </section>
    </main>
  );
};

export default About;
