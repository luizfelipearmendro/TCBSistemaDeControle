
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

document.addEventListener("DOMContentLoaded", function () {
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

   
    animacaoRolagem.style.height = `${alturaDocumento}px`;
    principalAnimacaoRolagem.style.height = `${alturaDocumento}px`;

    
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

function estaElementoNaTela(el) {
    const retangulo = el.getBoundingClientRect();
    return (
        retangulo.top >= 0 &&
        retangulo.left >= 0 &&
        retangulo.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        retangulo.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

function aplicarAnimacoesScroll() {
    const elementos = document.querySelectorAll('.animar-com-scroll');
    elementos.forEach((elemento) => {
        if (estaElementoNaTela(elemento)) {
            elemento.classList.add('ativo');
        }
    });
}

document.addEventListener('DOMContentLoaded', () => {
    aplicarAnimacoesScroll();
});

window.addEventListener('scroll', () => {
    aplicarAnimacoesScroll();
});