// Função para controlar a exibição do rodapé e o cabeçalho durante a rolagem
function rolagemRodape(posicaoRolagem, alturaRodape) {
    const rodape = document.querySelector('footer');
    const cabecalho = document.querySelector('.cabecalho-box-one');
    const conteudo = document.querySelector('.conteúdo');

    if (!rodape || !cabecalho || !conteudo) {
        return;
    }

    // Controla a visibilidade do rodapé
    if (posicaoRolagem >= alturaRodape) {
        rodape.style.bottom = '0px';
    } else {
        rodape.style.bottom = `-${alturaRodape}px`;
    }

    // Adiciona/remova a classe 'scroll-active' no cabeçalho
    if (posicaoRolagem >= conteudo.offsetTop) {
        cabecalho.classList.add('scroll-active');
    } else {
        cabecalho.classList.remove('scroll-active');
    }
}

// Espera o DOM carregar completamente
document.addEventListener("DOMContentLoaded", function () {
    // Obtém os elementos necessários
    const alturaJanela = window.innerHeight;
    const rodape = document.querySelector('footer');
    const conteudo = document.querySelector('.conteúdo');
    const animacaoRolagem = document.getElementById('rolagem-animação');
    const principalAnimacaoRolagem = document.getElementById('principal-rolagem-animação');
    const cabecalho = document.querySelector('header');
    const invólucroParalaxe = document.querySelector('.invólucro-paralaxe');

    if (!rodape || !conteudo || !animacaoRolagem || !principalAnimacaoRolagem || !cabecalho || !invólucroParalaxe) {
        console.error("Elemento necessário não encontrado no DOM.");
        return;
    }

    const alturaRodape = rodape.offsetHeight;
    const alturaConteudo = conteudo.offsetHeight;
    const alturaDocumento = alturaJanela + alturaConteudo + alturaRodape - 20;

    // Define as dimensões dos elementos para animação
    animacaoRolagem.style.height = `${alturaDocumento}px`;
    principalAnimacaoRolagem.style.height = `${alturaDocumento}px`;

    // Define o tamanho do cabeçalho e ajusta o invólucro paralaxe
    cabecalho.style.height = `${alturaJanela}px`;
    invólucroParalaxe.style.marginTop = `${alturaJanela}px`;

    // Chama a função inicialmente para configurar o estado inicial
    rolagemRodape(window.scrollY, alturaRodape);

    // Atualiza o estado durante a rolagem
    window.addEventListener('scroll', function () {
        const posicaoRolagem = window.scrollY;

        // Move o elemento principal de animação para criar o efeito de paralaxe
        principalAnimacaoRolagem.style.top = `-${posicaoRolagem}px`;

        // Ajusta a posição do fundo do cabeçalho
        const posicaoFundo = Math.max(0, 50 - (posicaoRolagem * 100 / alturaDocumento));
        cabecalho.style.backgroundPositionY = `${posicaoFundo}%`;

        // Atualiza a visibilidade do rodapé e o estado do cabeçalho
        rolagemRodape(posicaoRolagem, alturaRodape);
    });

});

// Função para verificar se um elemento está visível na tela
function isElementInViewport(el) {
    const rect = el.getBoundingClientRect();
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

// Função para aplicar animações aos elementos
function applyScrollAnimations() {
    const elements = document.querySelectorAll('.animate-on-scroll');
    elements.forEach((element) => {
        if (isElementInViewport(element)) {
            element.classList.add('active');
        }
    });
}

// Chama a função inicialmente para verificar elementos já visíveis
document.addEventListener('DOMContentLoaded', () => {
    applyScrollAnimations();
});

// Monitora a rolagem para aplicar animações dinamicamente
window.addEventListener('scroll', () => {
    applyScrollAnimations();
});