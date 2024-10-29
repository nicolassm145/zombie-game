# Zobware

## Descrição Geral
**Zobware** é um jogo de ação e sobrevivência ambientado nos anos 60. O jogador assume o papel de um fazendeiro solitário que, em uma noite, acorda com sons estranhos e descobre zumbis emergindo em sua fazenda. O objetivo é sobreviver a hordas crescentes de zumbis, utilizando armas, pontos e melhorias para enfrentar as ameaças.

**Versão da Unity:** `2022.3.50f1`

**Tileset utilizado:** O jogo utiliza o tileset **Zombie Apocalypse Tileset**, desenvolvido por **Ittai Manero**, disponível em [https://ittaimanero.itch.io/zombie-apocalypse-tileset](https://ittaimanero.itch.io/zombie-apocalypse-tileset).

---

## Mecânicas do Jogo

### Personagem Principal
- **Perfil:** O jogador controla um fazendeiro.
- **Vida:** O jogador começa com 100 de vida (ajustável).
- **Pontos:** Começa com 500 pontos, que podem ser usados para comprar armas e abrir áreas (ajustável).
- **Movimentos:** Andar, correr, atirar, recarregar e interagir.
  - **Corrida e tiro:** O jogador para de correr ao atirar, mas pode continuar andando.
- **Armas:** Inicia com uma pistola no round 1 e pode carregar até 2 armas.
- **Progressão:** Ganha pontos ao eliminar zumbis. A pontuação pode variar dependendo de fatores como vida do zumbi ou precisão do tiro.

### Armas
- **Categorias de Armas:** Pistola, submetralhadora, fuzil automático, escopeta, fuzil semiautomático.
- **Compra de Armas:**
  - Disponíveis em locais específicos no mapa.
  - Comprar uma arma repetida recarrega a munição ao máximo.
- **Caixa de Armas Aleatórias:**
  - Disponível no mapa, oferece armas aleatórias.
  - Pode conter uma arma exclusiva, que não pode ser comprada diretamente.
  - A posição da caixa muda após certo número de compras.
- **Máquina de Melhorias:**
  - Aumenta o dano, precisão e capacidade de munição.
  - Melhoria disponível em até 3 níveis.
  - Custo pago com pontos.

---

## Inimigos

### Zumbis
- **Comportamento:** Zumbis se movem mais rápido que o jogador andando, sempre na direção do jogador.
- **Aparição:** Nascem do chão, geralmente atrás de barreiras no mapa.
- **Dano:** Cada ataque causa 25 de dano ao jogador (ajustável).
- **Escalonamento:** A vida dos zumbis cresce logaritmicamente por round, assim como o número de zumbis vivos simultaneamente.
- **Quantidades:** O número total de zumbis por round cresce linearmente.

---

## Mapa

### Ambientação
- **Cenário:** O jogo se passa em uma fazenda sombria dos anos 60, cercada de árvores mortas e folhagens secas, à beira de uma estrada deserta com um posto de gasolina.
- **Detalhes Visuais:** A atmosfera do mapa é marcada por cores frias e ambientes lúgubres, complementando o cenário de uma invasão zumbi.
- **Tileset:** O visual do mapa é construído utilizando o **Zombie Apocalypse Tileset** de **Ittai Manero**.

### Interatividade
- **Exploração:** O jogador pode abrir portas, cercas ou remover destroços ao gastar pontos.
- **Construção:** Barreiras podem ser erguidas em pontos específicos para impedir o avanço dos zumbis.
- **Energia:** O jogador pode ligar a energia do mapa, liberando o uso da máquina de melhorias.

---

## Itens Coletáveis

Zumbis mortos podem deixar itens no chão, que o jogador coleta ao passar por cima deles. Eles permanecem no chão por até 2 minutos (ajustável).

### Tipos de Coletáveis
- **Munição:** Recarrega toda a munição do jogador.
- **Nuke:** Mata instantaneamente todos os zumbis vivos.
- **Black Friday:** Reduz o preço de todas as compras por 90 segundos (ajustável).
- **Carpinteiro:** Reconstrói todas as barreiras no mapa.
- **Pontos:** Concede uma quantidade extra de pontos.
- **Dobradinha:** Dobra a quantidade de pontos ganhos por 90 segundos (ajustável).
- **Único:** Permite matar zumbis com apenas um tiro por 90 segundos (ajustável).

---

## Vantagens

As vantagens podem ser compradas em pontos específicos no mapa para aprimorar o desempenho do jogador.

### Tipos de Vantagens
- **Força:** Aumenta a vida do jogador para 150 (ajustável).
- **Veloz:** Aumenta a velocidade de corrida.
- **Vontade:** Permite carregar 3 armas ao invés de 2.
- **Reanimação:** O jogador se revive no spawn após morrer, mas perde todas as vantagens. As armas e os pontos são mantidos.
- **Retorno:** Causa dano ao zumbi que ataca o jogador.
- **Potência:** Aumenta o dano das armas.

---

## Easter Egg

O jogo contém uma sequência de passos que os jogadores devem descobrir para avançar na história secreta. Ao completar todos os passos, o jogador descobre que a invasão zumbi não passa de um sonho de um universitário ansioso com uma prova.

---

## Considerações Técnicas
- **Unity:** Este jogo foi desenvolvido utilizando a versão **Unity 2022.3.50f1**.
- **Tileset:** O tileset utilizado é o **Zombie Apocalypse Tileset**, desenvolvido por **Ittai Manero**, e pode ser encontrado em [https://ittaimanero.itch.io/zombie-apocalypse-tileset](https://ittaimanero.itch.io/zombie-apocalypse-tileset).
