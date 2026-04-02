## My First Contact with .NET

<br/>

<p>Its a simple project made in <strong>.NET 8</strong>, using EntityFramework and SQLServer. <br/>Consists in 2 web APIs (Vendas and Estoque), where you can add products, close orders and handle the stock.</p>

<div display="flex" align="center">
    <img heigth="50" width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/_net_core.png"></img>    
    <img heigth="50" width="50" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/entityframeworkcore/entityframeworkcore-original.svg" />
    <img heigth="50" width="50" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" />
</div>


### Getting started

<p>Inside the folders <u>\docker-rabbitmq</u> and <u>\docker-sqlserver</u> you can find the docker-compose.yaml files. To run each of them:</p>

```bash
docker compose up -D
```

<p>Then you can run both web APIs projects. You can use VS Code or Visual Studio to see the code better and compile. The swagger shows you the endpoints in browser very well to test.</p>

</br>

### Endpoints

<strong>Produto:</strong>

| Method |  Endpoint  |
|:-----|:--------:|
| GET   | `/api/Produto` |
| POST   |  `/api/Produto`  |
| GET   | `/api/Produto/{id}` |
| PUT   | `/api/Produto/{id}` |

</br>
<strong>Pedido:</strong>

| Method |  Endpoint  |
|:-----|:--------:|
| POST   | `/api/Pedido` |
| GET   |  `/api/Pedido`  |
| DELETE   | `/api/Pedido/{id}` |
| GET   | `/api/Pedido/{id}` |

</br>
<strong>Estoque:</strong>

| Method |  Endpoint  |
|:-----|:--------:|
| GET   | `/api/Estoque` |
| POST   |  `/api/Estoque`  |
| GET   | `/api/Estoque/{id}` |
| GET   | `/api/Estoque/quantidade/{id}` |