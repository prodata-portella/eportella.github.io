# Seja bem vindo!

Eu sou a Portella Ltda.

Conheça a minha [trajetória](trajetoria/README.md).\
Tenho uma [missão](missao/README.md), [visão](visao/README.md) e [valores](valor/README.md) bem definidos, e sou regido pela minha compreensão abstraída do [*Scrum*](scrum/README.md), acreditando que ele está em coerência com o [Manifesto ágil](agile-manifesto/README.md).\
Organizo meus artefatos principalmente com três “tecnologias”:
1. [*Domain Driven Design*](domain-driven-design/README.md);
1. [*Feature Folder*](feature-folder/README.md);
1. [*Feature Toggles*](feature-toggles/README.md),

Se tiver interesse em ver na prática, acesse o [código fonte](https://github.com/eportella/PORTELLA-LTDA/tree/main/apresentacao) desta apresentação. Ele segue quase tudo[^1] o que foi mencionado acima e está disponível publicamente.

>[!NOTE]
>
>- Tenho uma semântica bem peculiar para implementar soluções. Tentarei descrever em breve.

Muito prazer!\
**PORTELLA-LTDA**\
[ewerton.portella@outlook.com](mailto:ewerton.portella@outlook.com)

[^1]: Exceto o *Feature Toggles* por se tratar de um site com natureza estática.


<script>
    const blockquoteFormat = () => {
        document.querySelectorAll('blockquote p').forEach(p =>{
            if(p.textContent === '[!NOTE]')
            {
                p.textContent = ''
                const note = document.createElement('<svg class="octicon octicon-info mr-2" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8Zm8-6.5a6.5 6.5 0 1 0 0 13 6.5 6.5 0 0 0 0-13ZM6.5 7.75A.75.75 0 0 1 7.25 7h1a.75.75 0 0 1 .75.75v2.75h.25a.75.75 0 0 1 0 1.5h-2a.75.75 0 0 1 0-1.5h.25v-2h-.25a.75.75 0 0 1-.75-.75ZM8 6a1 1 0 1 1 0-2 1 1 0 0 1 0 2Z"></path></svg>')
                t.appendChild(note);
                t.append('Note')
            }
        })
    };
    
    blockquoteFormat();
</script>