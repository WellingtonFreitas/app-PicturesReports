<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="GaleryPictures.aspx.cs" Inherits="PicturesReports.Views.GaleryPictures" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnGalery" runat="server" class="upnGalery">

        <ContentTemplate>
            <section>
                <div class="newGalery">
                    <div class="left">
                        <label>Nome</label>
                        <input id="txtName" type="text" runat="server" />
                        <label>Descrição</label>
                        <input id="txtDescription" type="text" runat="server" />
                    </div>
                    <div class="right">
                        <asp:FileUpload ID="pictureUpload" runat="server"></asp:FileUpload>
                        Ou
                    <asp:Button ID="OpenCam" runat="server" Text="Abrir Camera" OnClick="OpenCam_Click" />

                    </div>
                </div>

                <div id="footer" class="divFooter">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                </div>
            </section>
            <section>
                <asp:GridView ID="gvGalery" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowCommand="gvGalery_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome">
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%# Eval("Name") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Label ID="Description" runat="server" Text='<%# Eval("Description") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Capa Galeria">
                            <ItemTemplate>
                                <img src="<%# ReturnEncodedBase64UTF8(Eval("Cover")) %>" style="width: 100px;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Editar" CssClass="btn" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

    <!--------------------------------Modal de Editar------------------------------------------------>

    <asp:UpdatePanel ID="upnEdit" runat="server" Visible="false">
        <ContentTemplate>
            <section>
                <div>
                    <asp:Label ID="nameGalery" runat="server"></asp:Label>
                    <asp:Repeater ID="rptGaleryPictures" runat="server">
                        <ItemTemplate>
                            <img src="<%# ReturnEncodedBase64UTF8(Eval("ImageBinary")) %>" style="width: 300px;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div>
                    <div class="divFooter">
                        <asp:Button ID="btnAtualizar" runat="server" Text="Salvar" />
                        <asp:Button ID="btnSair" runat="server" Text="Voltar" />
                    </div>
                </div>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAtualizar" />
        </Triggers>
    </asp:UpdatePanel>

    <!--------------------------------Modal de Editar------------------------------------------------>

    <!--------------------------------Modal de Camera------------------------------------------------>
    <asp:UpdatePanel ID="upnWebCam" runat="server" Visible="false">
        <ContentTemplate>
            <section class="modal" style="z-index: 300;">
                <fieldset>
                    <div class="row-fluid">
                        <div class="modal-body" style="max-height: 600px;">
                            <video id="webCam" autoplay style="transform: scale(-1, 1);"></video>
                            <canvas id="canvas" class="d-none"></canvas>
                            <audio id="snapSound" src="audio/snap.wav" preload="auto"></audio>
                            <input id="pictureWebCam" runat="server" clientidmode="static" style="visibility: hidden" />
                        </div>
                    </div>
                    <div>
                        <div class="divFooter">
                            <asp:Button ID="btnSavePicture" runat="server" Text="Salvar" OnClick="btnSalvarFoto_Click" />
                            <asp:Button ID="btnCloseCam" runat="server" Text="Voltar" OnClick="btnFecharCamera_Click" />
                        </div>
                    </div>
                </fieldset>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSavePicture" />
        </Triggers>
    </asp:UpdatePanel>
    <!--------------------------------Modal de Camera------------------------------------------------>


</asp:Content>
