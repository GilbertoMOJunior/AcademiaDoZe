using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(MatriculaId), "Id")]
    public partial class MatriculaViewModel : BaseViewModel
    {
        public IEnumerable<EAppMatriculaPlano> PlanosMatricula { get; } = Enum.GetValues(typeof(EAppMatriculaPlano)).Cast<EAppMatriculaPlano>();

        private ObservableCollection<RestricaoItem> _restricoesMatricula;
        public ObservableCollection<RestricaoItem> RestricoesMatricula
        {
            get => _restricoesMatricula;
            set => SetProperty(ref _restricoesMatricula, value);
        }

        private readonly IMatriculaService _matriculaService;
        private readonly IAlunoService _alunoService;

        private static LogradouroDTO _logradouro = new()
        {
            Cep = string.Empty,
            Nome = string.Empty,
            Bairro = string.Empty,
            Cidade = string.Empty,
            Estado = string.Empty,
            Pais = string.Empty
        };

        private static AlunoDTO _aluno = new()
        {
            Nome = string.Empty,
            Cpf = string.Empty,
            DataNascimento = DateOnly.FromDateTime(DateTime.MinValue),
            Telefone = string.Empty,
            Endereco = _logradouro,
            Numero = string.Empty,
        };

        private MatriculaDTO _matricula = new()
        {
            AlunoMatricula = _aluno,
            Plano = EAppMatriculaPlano.Mensal,
            DataInicio = DateOnly.FromDateTime(DateTime.Now),
            DataFim = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            Objetivo = string.Empty,
            RestricoesMedicas = 0,
            ObservacoesRestricoes = string.Empty,
            LaudoMedico = new ArquivoDTO(),
        };

        public MatriculaDTO Matricula
        {
            get => _matricula;
            set
            {
                if (SetProperty(ref _matricula, value))
                {
                    // Notifica quando Matricula muda
                    OnPropertyChanged(nameof(DataFimCalculada));
                }
            }
        }

        // Propriedade auxiliar para o Plano com notificação
        public EAppMatriculaPlano PlanoSelecionado
        {
            get => Matricula.Plano;
            set
            {
                if (Matricula.Plano != value)
                {
                    Matricula.Plano = value;
                    AtualizarDataFim();
                }
            }
        }

        // Propriedade auxiliar para DataInicio com notificação
        public DateOnly DataInicioSelecionada
        {
            get => Matricula.DataInicio;
            set
            {
                if (Matricula.DataInicio != value)
                {
                    Matricula.DataInicio = value;
                    AtualizarDataFim();
                }
            }
        }

        public DateOnly DataFimCalculada
        {
            get
            {
                if (Matricula.DataInicio == DateOnly.MinValue)
                {
                    return DateOnly.FromDateTime(DateTime.Now);
                }

                return Matricula.Plano switch
                {
                    EAppMatriculaPlano.Mensal => Matricula.DataInicio.AddMonths(1),
                    EAppMatriculaPlano.Trimestral => Matricula.DataInicio.AddMonths(3),
                    EAppMatriculaPlano.Semestral => Matricula.DataInicio.AddMonths(6),
                    EAppMatriculaPlano.Anual => Matricula.DataInicio.AddYears(1),
                    _ => Matricula.DataInicio.AddMonths(1)
                };
            }
        }

        private int _MatriculaId;
        public int MatriculaId
        {
            get => _MatriculaId;
            set => SetProperty(ref _MatriculaId, value);
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public MatriculaViewModel(IMatriculaService matriculaService, IAlunoService alunoService)
        {
            _matriculaService = matriculaService;
            _alunoService = alunoService;
            Title = "Detalhes da Matricula";

            CarregarRestricoes();

            // Inicializa a data fim
            AtualizarDataFim();
        }

        private void AtualizarDataFim()
        {
            Matricula.DataFim = DataFimCalculada;
            OnPropertyChanged(nameof(DataFimCalculada));
        }

        private void CarregarRestricoes()
        {
            RestricoesMatricula = new ObservableCollection<RestricaoItem>();

            foreach (EAppMatriculaRestricoes restricao in Enum.GetValues(typeof(EAppMatriculaRestricoes)))
            {
                // Pula o valor "None"
                if (restricao == EAppMatriculaRestricoes.None)
                    continue;

                var item = new RestricaoItem
                {
                    Nome = restricao.GetDisplayName(),
                    Valor = restricao,
                    IsSelected = false
                };

                // Atualiza RestricoesMedicas quando um checkbox muda
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(RestricaoItem.IsSelected))
                    {
                        AtualizarRestricoesMedicas();
                    }
                };

                RestricoesMatricula.Add(item);
            }
        }

        private void AtualizarRestricoesMedicas()
        {
            EAppMatriculaRestricoes resultado = EAppMatriculaRestricoes.None;

            foreach (var item in RestricoesMatricula.Where(r => r.IsSelected))
            {
                resultado |= item.Valor;
            }

            Matricula.RestricoesMedicas = resultado;
        }

        private void CarregarRestricoesSelecionadas(int restricoesInt)
        {
            var restricoesEnum = (EAppMatriculaRestricoes)restricoesInt;

            foreach (var item in RestricoesMatricula)
            {
                item.IsSelected = restricoesEnum.HasFlag(item.Valor);
            }
        }

        /* métodos de comandos */

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task InitializeAsync()
        {
            if (MatriculaId > 0)
            {
                IsEditMode = true;
                Title = "Editar Matricula";
                await LoadMatriculaAsync();
            }
            else
            {
                IsEditMode = false;
                Title = "Nova Matricula";
            }
        }

        [RelayCommand]
        public async Task LoadMatriculaAsync()
        {
            if (MatriculaId <= 0)
                return;
            try
            {
                IsBusy = true;
                var MatriculaData = await _matriculaService.ObterPorIdAsync(MatriculaId);

                if (MatriculaData != null)
                {
                    Matricula = MatriculaData;

                    // Carrega as restrições selecionadas
                    CarregarRestricoesSelecionadas((int)Matricula.RestricoesMedicas);

                    // Atualiza a data fim com base nos dados carregados
                    AtualizarDataFim();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar Matricula: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveMatriculaAsync()
        {
            if (IsBusy)
                return;
            if (!ValidateMatricula(Matricula))
                return;
            try
            {
                IsBusy = true;

                // Atualiza as restrições médicas e data fim antes de salvar
                AtualizarRestricoesMedicas();
                AtualizarDataFim();

                // Verifica se o Aluno existe antes de continuar
                var alunoData = await _alunoService.ObterPorCpfAsync(Matricula.AlunoMatricula.Cpf);
                if (alunoData == null)
                {
                    await Shell.Current.DisplayAlert("Erro", "O Aluno informado não existe. O cadastro não pode continuar.", "OK");
                    return;
                }
                Matricula.AlunoMatricula = alunoData;

                if (IsEditMode)
                {
                    await _matriculaService.AtualizarAsync(Matricula);
                    await Shell.Current.DisplayAlert("Sucesso", "Matricula atualizado com sucesso!", "OK");
                }
                else
                {
                    await _matriculaService.AdicionarAsync(Matricula);
                    await Shell.Current.DisplayAlert("Sucesso", "Matricula criado com sucesso!", "OK");
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao salvar Matricula: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SearchByCpfAsync()
        {
            if (string.IsNullOrWhiteSpace(Matricula.AlunoMatricula.Cpf))
                return;
            try
            {
                IsBusy = true;
                var logradouroData = await _alunoService.ObterPorCpfAsync(Matricula.AlunoMatricula.Cpf);

                if (logradouroData != null)
                {
                    Matricula.AlunoMatricula = logradouroData;
                    OnPropertyChanged(nameof(Matricula));
                    await Shell.Current.DisplayAlert("Aviso", "Cpf encontrado! Aluno preenchido automaticamente.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "Cpf não encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar Cpf: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SelecionarLaudoAsync()
        {
            try
            {
                FileResult? result = null;
                result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione o Laudo",
                    FileTypes = FilePickerFileType.Pdf
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    Matricula.LaudoMedico = new ArquivoDTO { Conteudo = ms.ToArray() };
                    OnPropertyChanged(nameof(Matricula));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao selecionar laudo: {ex.Message}", "OK");
            }
        }

        private static bool ValidateMatricula(MatriculaDTO Matricula)
        {
            const string validationTitle = "Validação";

            if (string.IsNullOrWhiteSpace(Matricula.Objetivo))
            {
                Shell.Current.DisplayAlert(validationTitle, "Objetivo é obrigatório.", "OK");
                return false;
            }
            if (Matricula.AlunoMatricula is null)
            {
                Shell.Current.DisplayAlert(validationTitle, "Aluno é obrigatório", "OK");
                return false;
            }

            return true;
        }

        public class RestricaoItem : INotifyPropertyChanged
        {
            public string Nome { get; set; }
            public EAppMatriculaRestricoes Valor { get; set; }

            private bool _isSelected;
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    // Extension method para obter o nome do Display
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                ?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}