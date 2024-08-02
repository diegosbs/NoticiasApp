using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoticiasApp.Data;
using NoticiasApp.ViewModel;

public class NoticiaController : Controller
{
    private readonly AppDbContext _context;

    public NoticiaController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var noticias = await _context.Noticias.Include(n => n.Usuario).ToListAsync();
            return View(noticias);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    public IActionResult Create()
    {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
        ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Descricao");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,Texto,UsuarioId,SelectedTags")] NoticiaCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", viewModel.UsuarioId);
            ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Descricao");
            return View(viewModel);
        }

        try
        {
            var noticia = new Noticia
            {
                Titulo = viewModel.Titulo,
                Texto = viewModel.Texto,
                UsuarioId = viewModel.UsuarioId
            };

            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync();

            // Adicionar as tags associadas
            foreach (var tagId in viewModel.SelectedTags.Distinct())
            {
                _context.NoticiaTags.Add(new NoticiaTag
                {
                    NoticiaId = noticia.Id,
                    TagId = tagId
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", viewModel.UsuarioId);
            ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Descricao");
            return View(viewModel);
        }
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var noticia = await _context.Noticias
            .Include(n => n.NoticiaTags)
            .ThenInclude(nt => nt.Tag)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (noticia == null)
        {
            return NotFound();
        }

        var viewModel = new NoticiaEditViewModel
        {
            Id = noticia.Id,
            Titulo = noticia.Titulo,
            Texto = noticia.Texto,
            UsuarioId = noticia.UsuarioId,
            SelectedTags = noticia.NoticiaTags.Select(nt => nt.TagId).ToList()
        };

        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", noticia.UsuarioId);
        ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Descricao");

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Texto,UsuarioId,SelectedTags")] NoticiaEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", viewModel.UsuarioId);
            ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Descricao");
            return View(viewModel);
        }

        try
        {
            var noticia = await _context.Noticias
                .Include(n => n.NoticiaTags)
                .FirstOrDefaultAsync(n => n.Id == id);

            noticia.Titulo = viewModel.Titulo;
            noticia.Texto = viewModel.Texto;
            noticia.UsuarioId = viewModel.UsuarioId;

            // Atualizar as tags associadas
            var existingTags = noticia.NoticiaTags.Select(nt => nt.TagId).ToList();
            var newTags = viewModel.SelectedTags.Except(existingTags).ToList();
            var removedTags = existingTags.Except(viewModel.SelectedTags).ToList();

            // Adicionar novas tags
            foreach (var tagId in newTags)
            {
                _context.NoticiaTags.Add(new NoticiaTag
                {
                    NoticiaId = noticia.Id,
                    TagId = tagId
                });
            }

            // Remover tags não mais associadas
            foreach (var tagId in removedTags)
            {
                var tagToRemove = noticia.NoticiaTags.FirstOrDefault(nt => nt.TagId == tagId);
                if (tagToRemove != null)
                {
                    _context.NoticiaTags.Remove(tagToRemove);
                }
            }

            _context.Update(noticia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NoticiaExists(viewModel.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var noticia = await _context.Noticias
            .Include(n => n.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (noticia == null)
        {
            return NotFound();
        }

        return View(noticia);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var noticia = await _context.Noticias.FindAsync(id);
        _context.Noticias.Remove(noticia);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NoticiaExists(int id)
    {
        return _context.Noticias.Any(e => e.Id == id);
    }
}
