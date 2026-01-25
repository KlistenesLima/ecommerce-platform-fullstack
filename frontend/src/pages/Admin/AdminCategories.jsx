import { useState, useEffect } from 'react';
import { Container, Table, Button, Modal, Form, Spinner, Row, Col, Card } from 'react-bootstrap';
import { FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import { toast } from 'react-toastify';
import categoryService from '../../services/categoryService';

const AdminCategories = () => {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [saving, setSaving] = useState(false);

    // Estado para controlar edição (null = criando, objeto = editando)
    const [editingCategory, setEditingCategory] = useState(null);

    // Estado do formulário
    const [formData, setFormData] = useState({ name: '', description: '' });

    // Carregar categorias ao iniciar
    useEffect(() => {
        loadCategories();
    }, []);

    const loadCategories = async () => {
        try {
            setLoading(true);
            const data = await categoryService.getAll();
            setCategories(Array.isArray(data) ? data : []);
        } catch (error) {
            console.error('Erro ao carregar categorias:', error);
            toast.error('Erro ao carregar a lista de categorias.');
        } finally {
            setLoading(false);
        }
    };

    // Abrir modal (Limpa se for criar, preenche se for editar)
    const handleShow = (category = null) => {
        if (category) {
            setEditingCategory(category);
            setFormData({
                name: category.name,
                description: category.description || ''
            });
        } else {
            setEditingCategory(null);
            setFormData({ name: '', description: '' });
        }
        setShowModal(true);
    };

    const handleClose = () => setShowModal(false);

    // Enviar formulário (Criar ou Editar)
    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!formData.name.trim()) {
            toast.warning('O nome da categoria é obrigatório.');
            return;
        }

        try {
            setSaving(true);

            if (editingCategory) {
                // Atualizar
                const updated = await categoryService.update(editingCategory.id, formData);
                setCategories(categories.map(cat => cat.id === editingCategory.id ? updated : cat));
                toast.success('Categoria atualizada com sucesso!');
            } else {
                // Criar
                const created = await categoryService.create(formData);
                setCategories([...categories, created]);
                toast.success('Categoria criada com sucesso!');
            }

            handleClose();
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || 'Erro ao salvar categoria.');
        } finally {
            setSaving(false);
        }
    };

    // Deletar categoria
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja excluir esta categoria?')) {
            try {
                await categoryService.delete(id);
                setCategories(categories.filter(cat => cat.id !== id));
                toast.success('Categoria excluída.');
            } catch (error) {
                console.error(error);
                toast.error('Erro ao excluir categoria.');
            }
        }
    };

    return (
        <Container fluid className="p-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Gerenciar Categorias</h2>
                <Button variant="primary" onClick={() => handleShow()}>
                    <FiPlus className="me-2" /> Nova Categoria
                </Button>
            </div>

            {loading ? (
                <div className="text-center mt-5">
                    <Spinner animation="border" variant="primary" />
                </div>
            ) : (
                <Card className="shadow-sm">
                    <Card.Body className="p-0">
                        <Table responsive hover className="mb-0 align-middle">
                            <thead className="bg-light">
                                <tr>
                                    <th className="ps-4">Nome</th>
                                    <th>Descrição</th>
                                    <th className="text-end pe-4">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {categories.length > 0 ? (
                                    categories.map((category) => (
                                        <tr key={category.id}>
                                            <td className="ps-4 fw-bold">{category.name}</td>
                                            <td>{category.description || <em className="text-muted">Sem descrição</em>}</td>
                                            <td className="text-end pe-4">
                                                <Button
                                                    variant="outline-primary"
                                                    size="sm"
                                                    className="me-2"
                                                    onClick={() => handleShow(category)}
                                                >
                                                    <FiEdit2 />
                                                </Button>
                                                <Button
                                                    variant="outline-danger"
                                                    size="sm"
                                                    onClick={() => handleDelete(category.id)}
                                                >
                                                    <FiTrash2 />
                                                </Button>
                                            </td>
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan="3" className="text-center py-4 text-muted">
                                            Nenhuma categoria encontrada.
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </Table>
                    </Card.Body>
                </Card>
            )}

            {/* MODAL DE CADASTRO/EDIÇÃO */}
            <Modal show={showModal} onHide={handleClose} centered>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>{editingCategory ? 'Editar Categoria' : 'Nova Categoria'}</Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        <Form.Group className="mb-3" controlId="categoryName">
                            <Form.Label>Nome da Categoria</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Ex: Eletrônicos"
                                value={formData.name}
                                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                autoFocus
                            />
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="categoryDescription">
                            <Form.Label>Descrição</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                placeholder="Breve descrição da categoria"
                                value={formData.description}
                                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                            />
                        </Form.Group>
                    </Modal.Body>

                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>
                            Cancelar
                        </Button>
                        <Button variant="primary" type="submit" disabled={saving}>
                            {saving ? (
                                <>
                                    <Spinner as="span" animation="border" size="sm" role="status" aria-hidden="true" className="me-2" />
                                    Salvando...
                                </>
                            ) : (
                                editingCategory ? 'Atualizar' : 'Criar'
                            )}
                        </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        </Container>
    );
};

export default AdminCategories;