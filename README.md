
# Flexcomm

**Flexcomm** é um projeto de **E-Commerce flexível** para comercialização de **produtos e serviços**, desenvolvido com foco em **boas práticas de engenharia de software**.

---

## 🛠️ Tecnologias

- **Backend:** ASP.NET Core
- **Frontend:** Angular
- **Banco de Dados:** a definir
- **Containerização:** opcional com Docker
- **CI/CD:** GitHub Actions

---

## 🏛️ Arquitetura e Princípios

Este projeto adota as seguintes práticas e padrões:

- ✅ **DDD (Domain-Driven Design):** Modelagem orientada ao domínio, separando as responsabilidades em camadas distintas.
- ✅ **TDD (Test-Driven Development):** Desenvolvimento guiado por testes, garantindo qualidade e segurança.
- ✅ **Clean Code:** Código limpo, legível e de fácil manutenção.
- ✅ **Clean Architecture:** Estrutura organizada, com forte separação entre regras de negócio e infraestrutura.
- ✅ **SOLID:** Princípios de design orientados a objetos.

---

## 🎯 Objetivos

- Criar uma aplicação robusta e escalável para e-commerce de produtos e serviços.
- Garantir qualidade através de testes automatizados.
- Manter organização e padrões profissionais utilizando DDD e Clean Architecture.

---

## 📦 Estrutura do Projeto

```
/backend            # API ASP.NET Core seguindo DDD
/frontend-web      # SPA Angular para interface do cliente
/.github           # Workflows e configurações do GitHub Actions
/tests             # Testes unitários, integração e funcionais
```

---

## 🚀 Status

🚧 **Em desenvolvimento**  
- Inicialmente focado na criação da estrutura de backend e frontend.  
- Configuração de pipelines de CI/CD.  
- Implementação de testes automatizados.

---

## ✅ Como contribuir

1. Fork o projeto.
2. Crie uma branch: `feature/minha-nova-feature`.
3. Commit com Conventional Commits: `feat(core): adiciona nova funcionalidade`.
4. Envie um Pull Request.

---

## ⚠️ Requisitos

- .NET 8.0 ou superior
- Node.js 18.x ou superior
- Angular CLI
- Git

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

---

## 💡 Observações

- O deploy do frontend será realizado via **GitHub Pages**.
- O backend poderá ser hospedado inicialmente no **Render** com possibilidade futura de utilização de **Docker**.
