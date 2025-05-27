
# Contribuindo com o Flexcomm

Obrigado por considerar contribuir com o **Flexcomm**!

---

## ✅ Pré-requisitos

- .NET 8.0 ou superior
- Node.js 18.x ou superior
- Angular CLI
- Git

---

## ✅ Como rodar o projeto localmente

```bash
# Backend
cd backend
dotnet restore
dotnet run

# Frontend
cd frontend-web
npm install
ng serve
```

---

## ✅ Como contribuir

1. **Fork** o repositório.
2. Crie uma **branch** descritiva:

```bash
git checkout -b feature/nome-da-sua-feature
```

3. Siga o padrão de **commits convencionais**:

```bash
feat(core): adiciona nova funcionalidade
fix(api): corrige erro de validação
chore(ci): adiciona novo workflow
```

4. Escreva **testes automatizados**.
5. Confirme que o **CI/CD** passa.
6. Abra um **Pull Request** para a branch `develop`.

---

## ✅ Regras importantes

- Sempre use **Pull Request** → merge direto é bloqueado.
- O **CI** deve passar para o PR ser aceito.
- Mantenha o código **limpo** e siga **Clean Code**.
- Respeite a arquitetura **DDD**.

---

## ✅ Padrões de commit (Conventional Commits)

| Tipo      | Descrição                               |
|----------- |-------------------------------------- |
| `feat`    | Nova funcionalidade                   |
| `fix`     | Correção de bug                       |
| `chore`   | Tarefa de manutenção                  |
| `refactor`| Refatoração sem alteração funcional   |
| `docs`    | Documentação                          |
| `test`    | Adição ou alteração de testes         |

---

## ✅ Código de Conduta

- Seja respeitoso e colaborativo.
- Reporte problemas e proponha melhorias de forma construtiva.

---

## ✅ Licença

Este projeto está sob a licença [MIT](LICENSE).
